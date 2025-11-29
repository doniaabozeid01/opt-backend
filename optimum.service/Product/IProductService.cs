using optimum.data.Entities;
using optimum.service.Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllAsync();
        Task<ProductReadDto?> GetByIdAsync(int id);
        Task<ProductReadDto> AddAsync(ProductCreateDto dto);
        Task<ProductReadDto?> UpdateAsync(ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);

    }

}
