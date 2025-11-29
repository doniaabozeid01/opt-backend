using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.Product.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optimum.service.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =============================
        // GET ALL
        // =============================
        public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Repository<Products>().GetAllAsync();

            return products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Keywords = p.Keywords,
                CreatedAt = p.CreatedAt
            });
        }

        // =============================
        // GET BY ID
        // =============================
        public async Task<ProductReadDto?> GetByIdAsync(int id)
        {
            var p = await _unitOfWork.Repository<Products>().GetByIdAsync(id);
            if (p == null) return null;

            return new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Keywords = p.Keywords,
                CreatedAt = p.CreatedAt
            };
        }

        // =============================
        // ADD
        // =============================
        public async Task<ProductReadDto> AddAsync(ProductCreateDto dto)
        {
            var product = new Products
            {
                Name = dto.Name,
                Description = dto.Description,
                Keywords = dto.Keywords,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _unitOfWork.Repository<Products>().AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Keywords = product.Keywords,
                CreatedAt = product.CreatedAt
            };
        }

        // =============================
        // UPDATE
        // =============================
        public async Task<ProductReadDto?> UpdateAsync(ProductUpdateDto dto)
        {
            var product = await _unitOfWork.Repository<Products>().GetByIdAsync(dto.Id);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Keywords = dto.Keywords;

            _unitOfWork.Repository<Products>().Update(product);
            await _unitOfWork.CompleteAsync();

            return new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Keywords = product.Keywords,
                CreatedAt = product.CreatedAt
            };
        }

        // =============================
        // DELETE
        // =============================
        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _unitOfWork.Repository<Products>().GetByIdAsync(id);
            if (p == null) return false;

            await _unitOfWork.Repository<Products>().DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }



}













