using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.Supplier.Dtos;

namespace optimum.service.Supplier
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuppliersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<SupplierDto>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<Suppliers>();
            var suppliers = await repo.GetAllAsync();

            return suppliers
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    SupplierName = s.SupplierName,
                    ResponsiblePerson = s.ResponsiblePerson,
                    ContactEmail = s.ContactEmail,
                    Phone = s.Phone,
                    CreatedAt = s.CreatedAt
                })
                .ToList();
        }

        public async Task<SupplierDto> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Suppliers>();
            var s = await repo.GetByIdAsync(id);
            if (s == null) return null;

            return new SupplierDto
            {
                Id = s.Id,
                SupplierName = s.SupplierName,
                ResponsiblePerson = s.ResponsiblePerson,
                ContactEmail = s.ContactEmail,
                Phone = s.Phone,
                CreatedAt = s.CreatedAt
            };
        }

        public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
        {
            var repo = _unitOfWork.Repository<Suppliers>();

            var entity = new Suppliers
            {
                SupplierName = dto.SupplierName,
                ResponsiblePerson = dto.ResponsiblePerson,
                ContactEmail = dto.ContactEmail,
                Phone = dto.Phone,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new SupplierDto
            {
                Id = entity.Id,
                SupplierName = entity.SupplierName,
                ResponsiblePerson = entity.ResponsiblePerson,
                ContactEmail = entity.ContactEmail,
                Phone = entity.Phone,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto)
        {
            var repo = _unitOfWork.Repository<Suppliers>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.SupplierName = dto.SupplierName;
            entity.ResponsiblePerson = dto.ResponsiblePerson;
            entity.ContactEmail = dto.ContactEmail;
            entity.Phone = dto.Phone;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Suppliers>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
