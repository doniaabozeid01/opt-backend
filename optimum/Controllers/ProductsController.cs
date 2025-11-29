using Microsoft.AspNetCore.Mvc;
using optimum.service.Product;
using optimum.service.Product.Dtos; // لو DTOs في Folder اسمه Dtos

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // =============================
        // GET ALL
        // =============================
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        // =============================
        // GET BY ID
        // =============================
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound("Product Not Found");
            return Ok(product);
        }

        // =============================
        // ADD
        // =============================
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ProductCreateDto dto)
        {
            var product = await _service.AddAsync(dto);
            return Ok(product); // بيرجع ProductReadDto
        }

        // =============================
        // UPDATE
        // =============================
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(dto);

            if (updated == null)
                return NotFound("Product Not Found");

            return Ok(updated);
        }

        // =============================
        // DELETE
        // =============================
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound("Product Not Found");

            return Ok("Deleted Successfully");
        }
    }
}
