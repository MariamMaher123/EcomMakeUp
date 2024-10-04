using EcomMakeUp.Dtos;
using EcomMakeUp.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomMakeUp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AcountController : ControllerBase
    {
        //vvvvvvvvvvvvvvv
        private readonly IAuthServies _authServies;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public AcountController(IAuthServies authServies, IWebHostEnvironment webHostEnvironment, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _authServies = authServies;
            _webHostEnvironment = webHostEnvironment;
            _environment = environment;
            _configuration = configuration;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Account ()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if(userEmail == null) {

                return NotFound("Error in your data");
            }

            var user = await _authServies.SelectUserByEmail(userEmail);
            if(user == null)
            {
                return NotFound("Error in your data");

            }
            return Ok(user);

        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail == null)
            {
                return BadRequest("Error");
            }
            dto.Email=userEmail;
            var res = await _authServies.changePassword(dto);

            if (!string.IsNullOrEmpty(res.Message))
            {
                return BadRequest(res.Message);
            }

            return Ok("Success change password");


            }


        [Authorize]
        [HttpPut]

        public async Task<IActionResult>  UpdateProfile ([FromForm]UpdateProfileDTO dto)
        {
            var userEmail =  User.FindFirst(ClaimTypes.Email)?.Value;
           
            if (userEmail == null)
            {
                return BadRequest("Error");
            }

            var user =await _authServies.SelectUserByEmail(userEmail);
            if (user == null)
            {
                return BadRequest("Error");
            }

            user.Email=dto.Email;
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.Phone;
            user.Sex = dto.Sex;
            user.Nationality = dto.Nationality;
            user.BDay = dto.BDay;


            var scheme = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host;
            string fileExtension = Path.GetExtension(dto.Photo.FileName).ToLowerInvariant();


            //chech 
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Invalid file extension.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Photo.FileName);
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images"); // Path to uploads folder
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                dto.Photo.CopyTo(stream);
            }

            var imageUrl = $"{scheme}://{host}/images/{fileName}";

            user.Photo = fileName;

            var res = await _authServies.updateUser(user);

            if(!string.IsNullOrEmpty(res.Message))
            {
                return NotFound(res.Message);
            }
            return Ok("Success Update Data");



        }
    }

        


}
