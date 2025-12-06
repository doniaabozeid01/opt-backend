using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using optimum.service.Schools.Dtos;
using optimum.service.Schools;
using Microsoft.AspNetCore.Identity;
using optimum.data.Entities;
using optimum.service.Authentication.Dtos;
using optimum.service.Authentication;
using optimum.service.SupplierRate.Dtos;
using optimum.service.SupplierRate;
using optimum.service.SupplierRate;

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        private readonly ILogger<SchoolsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;
        readonly IRatingSupplierService _ratingSupplierService;

        public SchoolsController(ISchoolService schoolService,IRatingSupplierService ratingSupplierService , ILogger<SchoolsController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IJwtTokenService jwtTokenService)
        {
            _ratingSupplierService = ratingSupplierService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
        
            _schoolService = schoolService;
            _logger = logger;
        }


        // POST: api/schools
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateSchoolDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _schoolService.CreateSchoolAsync(dto, cancellationToken);
                // لو عايزة تبعتي object بسيط بدل الـ entity، ممكن ترجع DTO مخصص
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating school");
                return StatusCode(500, new { message = "An error occurred while creating the school." });
            }
        }

        // DELETE: api/schools/{id}
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _schoolService.DeleteSchoolAsync(id, cancellationToken);
            if (!deleted) return NotFound(new { message = "School not found." });
            return NoContent();
        }

        // اختياري — GET by id عشان CreatedAtAction يشتغل بشكل صحيح
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var school = await _schoolService.GetSchoolByIdAsync(id, cancellationToken);
            if (school == null) return NotFound();
            return Ok(school);
        }




        //[HttpPost("RegisterSchool")]
        //public async Task<IActionResult> RegisterSchool([FromBody] RegisterSchoolDto dto, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var school = await _schoolService.RegisterWithSchoolAsync(dto, cancellationToken);
        //        return Ok(new
        //        {
        //            school.Id,
        //            school.FullName,
        //            school.Code,
        //            school.UserId
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}








        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterSchoolDto dto)
        //{
        //    try
        //    {
        //        var (school, token) = await _schoolService.RegisterSchoolAsync(dto);
        //        return Ok(new
        //        {
        //            Message = "Registration successful",
        //            Token = token,
        //            UserId = school.UserId,
        //            SchoolId = school.Id,
        //            SchoolName = school.FullName,
        //            SchoolCode = school.Code
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}













        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterSchoolDto dto)
        {
            try
            {
                // ✅ نفس تعاليم UserRegister / SupplierRegister
                var emailError = _jwtTokenService.ValidateEmail(dto.Email);
                if (emailError != null)
                    return BadRequest(emailError);

                var (school, token, identityResult) = await _schoolService.RegisterSchoolAsync(dto);

                if (!identityResult.Succeeded)
                {
                    if (identityResult.Errors.Any(e => e.Code == "DuplicateUserName" || e.Code == "DuplicateEmail"))
                        return BadRequest("This email is already exist.");
                    else if (identityResult.Errors.Any(e => e.Code == "PasswordTooWeak"))
                        return BadRequest("Password is too weak.");
                    else if (identityResult.Errors.Any(e => e.Code == "InvalidEmail"))
                        return BadRequest("The email format is invalid.");
                    else
                        return BadRequest(new
                        {
                            Message = "Registration failed",
                            Errors = identityResult.Errors.Select(e => e.Description)
                        });
                }

                return Ok(new
                {
                    Message = "Registration successful",
                    Token = token,
                    UserId = school.UserId,
                    SchoolId = school.Id,
                    SchoolName = school.FullName,
                    SchoolCode = school.Code
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }




        [HttpPost("rate-supplier")]
        public async Task<IActionResult> RateSupplier([FromBody] SupplierRatingCreateDto dto)
        {
            await _ratingSupplierService.RateSupplierAsync(dto);
            return Ok();
        }













    }
}
