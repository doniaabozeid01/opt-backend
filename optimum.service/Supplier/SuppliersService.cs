using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.Authentication;
using optimum.service.Schools.Dtos;
using optimum.service.Supplier.Dtos;

namespace optimum.service.Supplier
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Suppliers> _repository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SuppliersService(IUnitOfWork unitOfWork,IGenericRepository<Suppliers> repository, RoleManager<IdentityRole> roleManager,
                    UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _repository = repository;
            _roleManager = roleManager;
        }

        public async Task<IReadOnlyList<SupplierDto>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<Suppliers>();
            var suppliers = await repo.GetAllAsync();

            return suppliers
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    ResponsiblePerson = s.ResponsiblePerson,
                    ContactEmail = s.ContactEmail,
                    Code = s.Code,
                    UserId = s.UserId,
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
                FullName = s.FullName,
                ResponsiblePerson = s.ResponsiblePerson,
                ContactEmail = s.ContactEmail,
                UserId= s.UserId,
                Code = s.Code,
                Phone = s.Phone,
                CreatedAt = s.CreatedAt
            };
        }

        public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
        {
            var repo = _unitOfWork.Repository<Suppliers>();

            var entity = new Suppliers
            {
                FullName = dto.FullName,
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
                FullName = entity.FullName,
                ResponsiblePerson = entity.ResponsiblePerson,
                ContactEmail = entity.ContactEmail,
                Phone = entity.Phone,
                Code = entity.Code,
                UserId = entity.UserId,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto)
        {
            var repo = _unitOfWork.Repository<Suppliers>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.FullName = dto.FullName;
            entity.ResponsiblePerson = dto.ResponsiblePerson;
            entity.ContactEmail = dto.ContactEmail;
            entity.Phone = dto.Phone;
            entity.Code = dto.Code;

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





















        public async Task<(Suppliers supplier, string token)> RegisterSupplierAsync(RegisterSupplierDto dto)
        {
            // ======= Email Validation =======
            var email = dto.Email;
            if (email.Contains(' '))
                throw new Exception("The email format shouldn't have any spaces.");
            if (!email.Contains('@'))
                throw new Exception("The email format is invalid.");
            var domain = email.Split('@').Last();
            if (domain != domain.ToLower())
                throw new Exception("Invalid email format. The domain must be in lowercase.");
            if (!email.EndsWith("@gmail.com"))
                throw new Exception("Only Gmail accounts are allowed.");

            // ======= Create Identity User =======
            var user = new ApplicationUser
            {
                UserName = dto.Email.Split('@')[0],
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            // ======= Assign Role Client =======
            var role = "Supplier";
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);

            // ======= Create School =======
            var supplier = new Suppliers
            {
                FullName = dto.FullName,
                Code = dto.Code,
                UserId = user.Id
            };
            await _repository.AddAsync(supplier);
            await _unitOfWork.CompleteAsync();

            // ======= Generate Token =======
            var token = _jwtTokenService.GenerateJwtToken(user);

            return (supplier, token);
        }





    }
}
