using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using optimum.data.Context;
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
        readonly OptimumDbContext _context;

        public SuppliersService(OptimumDbContext context,IUnitOfWork unitOfWork,IGenericRepository<Suppliers> repository, RoleManager<IdentityRole> roleManager,
                    UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _context = context;
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





















        //public async Task<(Suppliers supplier, string token)> RegisterSupplierAsync(RegisterSupplierDto dto)
        //{
        //    // ======= Email Validation =======
        //    var email = dto.Email;
        //    if (email.Contains(' '))
        //        throw new Exception("The email format shouldn't have any spaces.");
        //    if (!email.Contains('@'))
        //        throw new Exception("The email format is invalid.");
        //    var domain = email.Split('@').Last();
        //    if (domain != domain.ToLower())
        //        throw new Exception("Invalid email format. The domain must be in lowercase.");
        //    if (!email.EndsWith("@gmail.com"))
        //        throw new Exception("Only Gmail accounts are allowed.");

        //    // ======= Create Identity User =======
        //    var user = new ApplicationUser
        //    {
        //        UserName = dto.Email.Split('@')[0],
        //        Email = dto.Email
        //    };

        //    var result = await _userManager.CreateAsync(user, dto.Password);
        //    if (!result.Succeeded)
        //    {
        //        var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
        //        throw new Exception(errors);
        //    }

        //    // ======= Assign Role Client =======
        //    var role = "Supplier";
        //    if (!await _roleManager.RoleExistsAsync(role))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole(role));
        //    }

        //    await _userManager.AddToRoleAsync(user, role);

        //    // ======= Create School =======
        //    var supplier = new Suppliers
        //    {
        //        FullName = dto.FullName,
        //        Code = dto.Code,
        //        UserId = user.Id
        //    };
        //    await _repository.AddAsync(supplier);
        //    await _unitOfWork.CompleteAsync();

        //    // ======= Generate Token =======
        //    var token = _jwtTokenService.GenerateJwtToken(user);

        //    return (supplier, token);
        //}



















        //public async Task<(Suppliers supplier, string token)> RegisterSupplierAsync(RegisterSupplierDto dto)
        //{
        //    // نستخدم Transaction عشان لو حاجة وقعت.. كله يترجع
        //    using var trx = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        // 1) إنشاء الـ Identity User (Email + Password فقط)
        //        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        //        if (existingUser != null)
        //            throw new Exception("Email already exists");

        //        var user = new ApplicationUser
        //        {
        //            UserName = dto.Email,
        //            Email = dto.Email
        //        };

        //        var createUserResult = await _userManager.CreateAsync(user, dto.Password);

        //        if (!createUserResult.Succeeded)
        //        {
        //            var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
        //            throw new Exception($"Failed to create user: {errors}");
        //        }

        //        // ممكن تضيفيه في Role "Supplier"
        //        // await _userManager.AddToRoleAsync(user, "Supplier");

        //        // 2) إنشاء سجل المورد في جدول Suppliers
        //        var supplier = new Suppliers
        //        {
        //            FullName = dto.FullName,
        //            Code = dto.Code,
        //            UserId = user.Id,
        //            ResponsiblePerson = dto.ResponsiblePerson,
        //            ContactEmail = dto.Email,
        //            Phone = dto.Phone,
        //            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
        //            SupplierProducts = new List<SupplierProducts>()
        //        };

        //        _context.Suppliers.Add(supplier);
        //        await _context.SaveChangesAsync(); // عشان الـ Id يتولد

        //        // 3) حفظ المنتجات اللي اختارها المورد في SupplierProducts
        //        if (dto.Products != null && dto.Products.Any())
        //        {
        //            foreach (var p in dto.Products)
        //            {
        //                var sp = new SupplierProducts
        //                {
        //                    SuppliersId = supplier.Id,
        //                    ProductsId = p.ProductId,
        //                    Price = p.Price,
        //                    Notes = p.Notes,
        //                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
        //                };

        //                _context.SupplierProducts.Add(sp);
        //            }

        //            await _context.SaveChangesAsync();
        //        }

        //        // 4) إنشاء Token للمورد الجديد
        //        var token = _jwtTokenService.GenerateJwtToken(user);

        //        await trx.CommitAsync();

        //        return (supplier, token);
        //    }
        //    catch
        //    {
        //        await trx.RollbackAsync();
        //        throw;
        //    }
        //}
















        public async Task<(Suppliers supplier, string token, IdentityResult identityResult)>
    RegisterSupplierAsync(RegisterSupplierDto dto)
        {
            using var trx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1) إنشاء Identity User
                var user = new ApplicationUser
                {
                    UserName = dto.Email.Split('@')[0],
                    Email = dto.Email
                };

                var result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                {
                    // نرجّع النتيجة للأكشن يتصرف فيها
                    return (null, null, result);
                }

                // ممكن تضيفيه في Role Supplier
                var role = "Supplier";
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(user, role);

                // 2) إنشاء Supplier
                var supplier = new Suppliers
                {
                    FullName = dto.FullName,
                    Code = dto.Code,
                    UserId = user.Id,
                    ResponsiblePerson = dto.ResponsiblePerson,
                    ContactEmail = dto.Email,
                    Phone = dto.Phone,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

                // 3) المنتجات
                if (dto.Products != null && dto.Products.Any())
                {
                    foreach (var p in dto.Products)
                    {
                        var sp = new SupplierProducts
                        {
                            SuppliersId = supplier.Id,
                            ProductsId = p.ProductId,
                            Price = p.Price,
                            Notes = p.Notes,
                            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
                        };

                        _context.SupplierProducts.Add(sp);
                    }

                    await _context.SaveChangesAsync();
                }

                // 4) التوكن
                var token = _jwtTokenService.GenerateJwtToken(user);

                await trx.CommitAsync();

                return (supplier, token, result);
            }
            catch
            {
                await trx.RollbackAsync();
                throw;
            }
        }









    }
}
