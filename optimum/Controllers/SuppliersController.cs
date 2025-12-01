using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using optimum.service.Supplier.Dtos;
using optimum.service.Supplier;
using optimum.service.Schools.Dtos;

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISuppliersService _suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        // GET: api/Suppliers
        [HttpGet("GetAllSuppliers")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll()
        {
            var suppliers = await _suppliersService.GetAllAsync();
            return Ok(suppliers);
        }

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
        [HttpPost("CreateNewSupplier")]
        public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _suppliersService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/Suppliers/5
        [HttpPut("UpdateExistingSupplier/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _suppliersService.UpdateAsync(id, dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("DeleteSupplier{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _suppliersService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }







        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterSupplierDto dto)
        {
            try
            {
                var (supplier, token) = await _suppliersService.RegisterSupplierAsync(dto);
                return Ok(new
                {
                    Message = "Registration successful",
                    Token = token,
                    UserId = supplier.UserId,
                    SchoolId = supplier.Id,
                    SchoolName = supplier.FullName,
                    SchoolCode = supplier.Code
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }








    }
}
