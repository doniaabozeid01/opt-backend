using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using optimum.data.Context;
using optimum.data.Entities;
using optimum.repository.Interfaces;
using optimum.service.Authentication;
using optimum.service.Schools.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.service.Schools
{
    public class SchoolService : ISchoolService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<School> _repository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        readonly IUnitOfWork _unitOfWork;
        readonly OptimumDbContext _context;

        public SchoolService(OptimumDbContext context,IUnitOfWork unitOfWork,IGenericRepository<School> repository, RoleManager<IdentityRole> roleManager,
                    UserManager<ApplicationUser> userManager ,IJwtTokenService jwtTokenService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _repository = repository;
            _roleManager = roleManager;
        }

        public async Task<School> CreateSchoolAsync(CreateSchoolDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.FullName)) throw new ArgumentException("FullName is required");
            if (string.IsNullOrWhiteSpace(dto.Code)) throw new ArgumentException("Code is required");

            var school = new School
            {
                FullName = dto.FullName.Trim(),
                Code = dto.Code.Trim()
            };

            return await _repository.AddAsync(school, cancellationToken);
        }

        public async Task<bool> DeleteSchoolAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository.DeleteAsync(id, cancellationToken);
        }

        public async Task<School> GetSchoolByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByIdAsync(id, cancellationToken);
        }



        //public async Task<School> RegisterWithSchoolAsync(RegisterSchoolDto dto, CancellationToken cancellationToken = default)
        //{
        //    // 1) Create Identity User
        //    var user = new ApplicationUser
        //    {
        //        UserName = dto.Email,
        //        Email = dto.Email
        //    };

        //    var result = await _userManager.CreateAsync(user, dto.Password);

        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(string.Join(" | ", result.Errors.Select(e => e.Description)));
        //    }

        //    // 2) Create School linked to this user
        //    var school = new School
        //    {
        //        FullName = dto.FullName,
        //        Code = dto.Code,
        //        UserId = user.Id
        //    };

        //    return await _repository.AddAsync(school, cancellationToken);
        //}



        //public async Task<(School school, string token)> RegisterSchoolAsync(RegisterSchoolDto dto)
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
        //    var role = "Client";
        //    if (!await _roleManager.RoleExistsAsync(role))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole(role));
        //    }

        //    await _userManager.AddToRoleAsync(user, role);

        //    // ======= Create School =======
        //    var school = new School
        //    {
        //        FullName = dto.FullName,
        //        Code = dto.Code,
        //        UserId = user.Id
        //    };
        //    await _repository.AddAsync(school);
        //    await _unitOfWork.CompleteAsync();

        //    // ======= Generate Token =======
        //    var token = _jwtTokenService.GenerateJwtToken(user);

        //    return (school, token);
        //}












        public async Task<(School school, string token, IdentityResult identityResult)>
    RegisterSchoolAsync(RegisterSchoolDto dto)
        {
            using var trx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1) إنشاء الـ Identity User
                var user = new ApplicationUser
                {
                    UserName = dto.Email.Split('@')[0],
                    Email = dto.Email
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    // مانكمّتش هنا؛ نرجّع IdentityResult للكنترولر يتصرف فيه
                    return (null, null, result);
                }

                // 2) إضافة Role "Client"
                var role = "Client";
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(user, role);

                // 3) إنشاء المدرسة وربطها باليوزر
                var school = new School
                {
                    FullName = dto.FullName,
                    Code = dto.Code,
                    UserId = user.Id,

                    ResponsiblePerson = dto.ResponsiblePerson,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    //ContactEmail = dto.Email   // أو ممكن تخليها حقل مختلف لو حابة
                };

                await _repository.AddAsync(school);
                await _unitOfWork.CompleteAsync();

                // 4) إنشاء JWT Token
                var token = _jwtTokenService.GenerateJwtToken(user);

                await trx.CommitAsync();

                return (school, token, result);
            }
            catch
            {
                await trx.RollbackAsync();
                throw;
            }
        }





    }

}
