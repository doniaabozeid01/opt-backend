using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using optimum.data.Entities;
using optimum.service.Authentication.Dtos;
using optimum.service.Authentication;

namespace optimum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    { 

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("userRegister")]
        public async Task<IActionResult> UserRegister(AuthDto registerDto)
        {
            var email = registerDto.Email;
            if (email.Contains(' '))
            {
                return BadRequest("The email format shouldn't have any spaces.");
            }
            // تحقق من وجود @ في البريد الإلكتروني
            if (!email.Contains('@'))
            {
                return BadRequest("The email format is invalid.");
            }

            // استخراج الجزء الذي بعد الـ "@" للتأكد أنه مكتوب بالكامل بحروف صغيرة
            var domain = email.Split('@').Last();  // النطاق بعد "@"

            if (domain != domain.ToLower())  // إذا كان النطاق يحتوي على أحرف كبيرة
            {
                return BadRequest("Invalid email format. The domain must be in lowercase.");
            }

            // التأكد من أن النطاق هو "@gmail.com" فقط
            if (!email.EndsWith("@gmail.com"))
            {
                return BadRequest("Only Gmail accounts are allowed.");
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email.Split('@')[0],
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // تعيين دور (اختياري)
                var role = "User";
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(user, role);

                // إنشاء التوكن
                var token = _jwtTokenService.GenerateJwtToken(user);

                return Ok(new
                {
                    Message = "Registration successful",
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email
                });
            }
            if (result.Errors.Any(e => e.Code == "DuplicateUserName" || e.Code == "DuplicateEmail"))
            {
                return BadRequest("This email is already exist.");
            }
            else if (result.Errors.Any(e => e.Code == "PasswordTooWeak"))
            {
                return BadRequest("Password is too weak.");
            }
            else if (result.Errors.Any(e => e.Code == "InvalidEmail"))
            {
                return BadRequest("The email format is invalid.");
            }
            else
            {
                return BadRequest(new { Message = "Registration failed", Errors = result.Errors.Select(e => e.Description) });
            }
        }






        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // إنشاء التوكن
                var token = _jwtTokenService.GenerateJwtToken(user);

                return Ok(new
                {
                    Message = "Login successful",
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email,
                    Role = _roleManager.FindByIdAsync(user.Id),
                });
            }

            return Unauthorized(new { Message = "Invalid email or password" });
        }
    }
}
