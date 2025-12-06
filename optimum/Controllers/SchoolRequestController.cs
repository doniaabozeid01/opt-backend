using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using optimum.data.Entities;
using optimum.data.Enum;
using optimum.repository.Interfaces;
using optimum.service.SchoolRequest.Dtos;
using optimum.service.TextRequestsParser;

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolRequestController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly ITextRequestParserService _aiParserService;

        public SchoolRequestController(IUnitOfWork unitOfWork, ITextRequestParserService aiParserService)
        {
            _unitOfWork = unitOfWork;
            _aiParserService = aiParserService; 
        }

        // POST: api/SchoolRequests
        [HttpPost("CreateByList")]
        public async Task<IActionResult> Create([FromBody] CreateSchoolRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.RequestType != RequestTypeEnum.Text && dto.RequestType != RequestTypeEnum.Form)
                return BadRequest("RequestType must be Text or Form in this endpoint.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // من التوكن

            var request = new SchoolRequests
            {
                SchoolId = dto.SchoolId,
                RequestType = dto.RequestType,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                //CreatedByUserId = userId
            };

            // لو Form وفيه Items
            if (dto.RequestType == RequestTypeEnum.Form && dto.Items != null && dto.Items.Any())
            {
                request.Items = dto.Items.Select(i => new SchoolRequestItems
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Notes = i.Notes
                }).ToList();
            }

            await _unitOfWork.Repository<SchoolRequests>().AddAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok(new { request.Id });
        }





        //// POST: api/SchoolRequests
        //[HttpPost("CreateFreeTxt")]
        //public async Task<IActionResult> CreateFreeTxt([FromBody] CreateSchoolRequestFreeTxtDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if (dto.RequestType != RequestTypeEnum.Text && dto.RequestType != RequestTypeEnum.Form)
        //        return BadRequest("RequestType must be Text or Form in this endpoint.");

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // من التوكن

        //    var request = new SchoolRequests
        //    {
        //        SchoolId = dto.SchoolId,
        //        RequestType = dto.RequestType,
        //        TextContent = dto.RequestType == RequestTypeEnum.Text ? dto.TextContent : null,
        //        Status = "Pending",
        //        CreatedAt = DateTime.UtcNow,
        //        //CreatedByUserId = userId
        //    };

        //    await _unitOfWork.Repository<SchoolRequests>().AddAsync(request);
        //    await _unitOfWork.CompleteAsync();

        //    return Ok(new { request.Id });
        //}






        [HttpPost("CreateFreeTxt")]
        public async Task<IActionResult> CreateFreeTxt([FromBody] CreateSchoolRequestFreeTxtDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.RequestType != RequestTypeEnum.Text)
                return BadRequest("RequestType must be Text in this endpoint.");

            var request = new SchoolRequests
            {
                SchoolId = dto.SchoolId,
                RequestType = dto.RequestType,
                TextContent = dto.TextContent,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<SchoolRequests>().AddAsync(request);
            await _unitOfWork.CompleteAsync();

            // 🔥 STEP 2: ANALYZE TEXT
            var parsedItems = await _aiParserService.ParseAsync(request);

            // 🔥 STEP 3: Save results to DB
            foreach (var item in parsedItems)
            {
                await _unitOfWork.Repository<AIParsedRequestItems>().AddAsync(item);
            }
            await _unitOfWork.CompleteAsync();


            // بعد ما تحفظي parsedItems في جدول AIParsedRequestItems
            var confirmRepo = _unitOfWork.Repository<SchoolConfirmedRequestItems>();

            foreach (var ai in parsedItems)
            {
                var confirmItem = new SchoolConfirmedRequestItems
                {
                    SchoolRequestId = ai.SchoolRequestId,
                    ProductId = ai.ProductId ?? 2,                      // اقتراح الـ AI إن وجد
                    ProductName = ai.Product?.Name ?? ai.ExtractedName,
                    Quantity = ai.Quantity,
                    Notes = ai.Notes,
                    IsConfirmed = false,
                    ConfirmedAt = DateTime.UtcNow,
                    AIParsedItemId = ai.Id                         // لو محتاجة تربطيهم
                };

                await confirmRepo.AddAsync(confirmItem);
            }

            await _unitOfWork.CompleteAsync();



            // Update status
            request.Status = "AI_Analyzed";
            _unitOfWork.Repository<SchoolRequests>().Update(request);
            await _unitOfWork.CompleteAsync();

            return Ok(new { request.Id, message = "Created and analyzed" });
        }





        // POST: api/SchoolRequests/file
        [HttpPost("CreateFromFile")]
        public async Task<IActionResult> CreateFromFile([FromForm] CreateSchoolRequestFileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("File is required.");

            if (dto.RequestType != RequestTypeEnum.File)
                return BadRequest("RequestType must be File in this endpoint.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // حفظ الملف في السيرفر
            var uploadsFolder = Path.Combine("Uploads", "SchoolRequests", "Files");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var request = new SchoolRequests
            {
                SchoolId = dto.SchoolId,
                RequestType = RequestTypeEnum.File,
                FilePath = filePath,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                //CreatedByUserId = userId
            };

            await _unitOfWork.Repository<SchoolRequests>().AddAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok(new { request.Id });
        }



        // POST: api/SchoolRequests/voice
        [HttpPost("CreateFromVoice")]
        public async Task<IActionResult> CreateFromVoice([FromForm] CreateSchoolRequestVoiceDto dto)
        {
            if (dto.Audio == null || dto.Audio.Length == 0)
                return BadRequest("Audio file is required.");

            if (dto.RequestType != RequestTypeEnum.Voice)
                return BadRequest("RequestType must be Voice in this endpoint.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var uploadsFolder = Path.Combine("Uploads", "SchoolRequests", "Audio");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Audio.FileName)}";
            var audioPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(audioPath, FileMode.Create))
            {
                await dto.Audio.CopyToAsync(stream);
            }

            var request = new SchoolRequests
            {
                SchoolId = dto.SchoolId,
                RequestType = RequestTypeEnum.Voice,
                AudioPath = audioPath,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                //CreatedByUserId = userId
            };

            await _unitOfWork.Repository<SchoolRequests>().AddAsync(request);
            await _unitOfWork.CompleteAsync();

            return Ok(new { request.Id });
        }
    }
}
