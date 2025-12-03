using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using optimum.data.Entities;
using optimum.service.Supplier.Dtos;

namespace optimum.service.Supplier
{
    public interface ISuppliersService
    {
        Task<IReadOnlyList<SupplierDto>> GetAllAsync();
        Task<SupplierDto> GetByIdAsync(int id);
        Task<SupplierDto> CreateAsync(CreateSupplierDto dto);
        Task<bool> UpdateAsync(int id, UpdateSupplierDto dto);
        Task<bool> DeleteAsync(int id);
        //Task<(Suppliers supplier, string token)> RegisterSupplierAsync(RegisterSupplierDto dto);
        Task<(Suppliers supplier, string token, IdentityResult identityResult)>
    RegisterSupplierAsync(RegisterSupplierDto dto);
    }
}
