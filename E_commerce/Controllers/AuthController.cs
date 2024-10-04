using EcomMakeUp.Dtos;
using EcomMakeUp.Models;
using EcomMakeUp.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcomMakeUp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthServies _authServies;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        public readonly IEmailSender _emailSender;


        public readonly UserManager<ApplicationUser> _userManager;
    
        private readonly RoleManager<IdentityRole> _roleManager;


        public AuthController(IAuthServies authServies, IWebHostEnvironment webHostEnvironment, IWebHostEnvironment environment, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _authServies = authServies;
            _webHostEnvironment = webHostEnvironment;
            _environment = environment;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById (string id)
        {
            return Ok(await _authServies.SelectUserById(id));   
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser ([FromForm]CreateUserDTO user)
        {
            if(ModelState.IsValid)
            {
                var user1 = new ApplicationUser
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.Phone,
                    Sex = user.Sex,
                    UserName = user.Email,
                    BDay = user.BDay,
                    Nationality = user.Nationality,
                   

                };
                var scheme = HttpContext.Request.Scheme;
                var host = HttpContext.Request.Host;
                string fileExtension = Path.GetExtension(user.Photo.FileName).ToLowerInvariant();


                //chech 
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file extension.");
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.Photo.FileName);
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images"); // Path to uploads folder
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    user.Photo.CopyTo(stream);
                }

                var imageUrl = $"{scheme}://{host}/images/{fileName}";
                user1.Photo = fileName;


                
                var ress = await _authServies.createUser(user1, user.Password) ;
              
                if (!ress.IsAuthenticated)
                {
                    return BadRequest(ress.Message);
                }
                return Ok(ress);

            }
            return BadRequest("you have wrong in your data. ");
        }



        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _authServies.listUsers());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersByName(string name)
        {
            return Ok(await _authServies.listUsersByName(name));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail (string email)
        {
            return Ok(await _authServies.SelectUserByEmail(email));
        }

        [HttpPost]
        public async Task<IActionResult> Login (LoginDTO model)
        {
            if(ModelState.IsValid)
            {
                var res=await _authServies.CheckUserLogin(model);
                if(!res.IsAuthenticated)
                {
                    return NotFound(res.Message);
                }
                return Ok(res);
            }
            return BadRequest("you have wrong in your data. ");
        }


        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleDto dto)
        {
            if(ModelState.IsValid)
            {
                var res = await _authServies.AddRole(dto);
                if(!string.IsNullOrEmpty(res))
                {
                    return BadRequest(res);
                }
                return Ok(dto);
            }
            return BadRequest("you have wrong in your data. ");
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // توليد كود مكون من 6 أرقام
            var resetCode = new Random().Next(100000, 999999).ToString();

            // تخزين الكود كـ Claim في Identity أو في قاعدة البيانات مع تاريخ انتهاء
            var result = await _userManager.SetAuthenticationTokenAsync(user, "Default", "PasswordResetCode", resetCode);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to generate reset code.");
            }

            // إرسال الكود عبر البريد الإلكتروني
            await _emailSender.SendEmailAsync(user.Email, "Reset Password Code",
                $"Your password reset code is: {resetCode}");

            return Ok("Password reset code has been sent to your email.");
        }




        [HttpPost]
        public async Task<IActionResult> VerifyResetCode(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid email.");
            }

            // Retrieve the stored reset code
            var storedCode = await _userManager.GetAuthenticationTokenAsync(user, "Default", "PasswordResetCode");

            if (storedCode == null || storedCode != code)
            {
                return BadRequest("Invalid or expired reset code.");
            }

            // Code is valid
            return Ok("Reset code is valid. You can now reset your password.");
        }




        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string code, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // استرجاع الكود المخزن باستخدام الـ UserManager
            var storedCode = await _userManager.GetAuthenticationTokenAsync(user, "Default", "PasswordResetCode");

            if (storedCode == null || storedCode != code)
            {
                return BadRequest("Invalid or expired reset code.");
            }

            // إعادة تعيين كلمة المرور
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                // مسح الكود بعد الاستخدام
                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "PasswordResetCode");
                return Ok("Password has been reset successfully.");
            }

            return BadRequest("Failed to reset password.");
        }




    }
}
