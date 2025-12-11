using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using optimum.service.Supplier.Dtos;
using optimum.service.Supplier;
using optimum.service.Schools.Dtos;
using optimum.service.Authentication;
using optimum.service.SupplierRequests;

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISuppliersService _suppliersService;
        readonly IJwtTokenService _jwtTokenService;
        private readonly ISupplierRequestService _supplierRequestService;

        public SuppliersController(ISupplierRequestService supplierRequestService,ISuppliersService suppliersService, IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
            _suppliersService = suppliersService;
            _supplierRequestService = supplierRequestService;

        }

        // GET: api/Suppliers
        [HttpGet("GetAllSuppliers")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll()
        {
            var suppliers = await _suppliersService.GetAllAsync();
            return Ok(suppliers);
        }




        // GET: api/SupplierRequests/by-supplier/5
        [HttpGet("SupplierRequestsBy-supplier/{supplierId:int}")]
        public async Task<IActionResult> GetBySupplier(int supplierId)
        {
            var requests = await _supplierRequestService.GetSupplierRequestsAsync(supplierId);
            return Ok(requests);
        }

        // لو بعدين هتجيبي الـ supplierId من الـ token:
        // [HttpGet("my-requests")]
        // public async Task<IActionResult> GetMyRequests() { ... }
    



        // GET: api/Suppliers/5
        [HttpGet("GetSupplierById/{id}")]
        public async Task<ActionResult<SupplierDto>> GetById(int id)
        {
            var supplier = await _suppliersService.GetByIdAsync(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        // POST: api/Suppliers
        //[HttpPost("CreateNewSupplier")]
        //public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var created = await _suppliersService.CreateAsync(dto);
        //    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        //}






        // PUT: api/Suppliers/5
        //[HttpPut("UpdateExistingSupplier/{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var success = await _suppliersService.UpdateAsync(id, dto);
        //    if (!success)
        //        return NotFound();

        //    return NoContent();
        //}







        // DELETE: api/Suppliers/5
        [HttpDelete("DeleteSupplier{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _suppliersService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }











        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterSupplierDto dto)
        //{
        //    try
        //    {
        //        var (supplier, token) = await _suppliersService.RegisterSupplierAsync(dto);
        //        return Ok(new
        //        {
        //            Message = "Registration successful",
        //            Token = token,
        //            UserId = supplier.UserId,
        //            SchoolId = supplier.Id,
        //            SchoolName = supplier.FullName,
        //            SchoolCode = supplier.Code
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}


















        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterSupplierDto dto)
        //{
        //    try
        //    {
        //        var (supplier, token) = await _suppliersService.RegisterSupplierAsync(dto);
        //        return Ok(new
        //        {
        //            Message = "Registration successful",
        //            Token = token,
        //            UserId = supplier.UserId,
        //            SupplierId = supplier.Id,
        //            SupplierName = supplier.FullName,
        //            SupplierCode = supplier.Code
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}








        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterSupplierDto dto)
        {
            try
            {
                // ✅ 1) نفس تعاليم الـ UserRegister
                var emailError = _jwtTokenService.ValidateEmail(dto.Email);
                if (emailError != null)
                    return BadRequest(emailError);

                // ✅ 2) ننده على الخدمة اللي بتسجل المورد
                var (supplier, token, identityResult) = await _suppliersService.RegisterSupplierAsync(dto);

                // ✅ 3) التعامل مع أخطاء Identity زي UserRegister
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

                // ✅ 4) لو كله تمام
                return Ok(new
                {
                    Message = "Registration successful",
                    Token = token,
                    UserId = supplier.UserId,
                    SupplierId = supplier.Id,
                    SupplierName = supplier.FullName,
                    SupplierCode = supplier.Code
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
