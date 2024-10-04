using EcomMakeUp.Dtos;
using EcomMakeUp.Models;
using EcomMakeUp.Servies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace EcomMakeUp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductSerives _ProductServies;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public ProductController(IProductSerives productServies, IWebHostEnvironment webHostEnvironment, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _ProductServies = productServies;
            _webHostEnvironment = webHostEnvironment;
            _environment = environment;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var prod = await _ProductServies.GetAllProducts();
            return Ok(prod);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct ([FromForm]AddProdDTO Prod)
        {
            var product = new Products
            {
                Name = Prod.Name,
                Description = Prod.Description,
                Price = Prod.Price,
                Brand = Prod.Brand
            };
            var scheme = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host;
            string fileExtension = Path.GetExtension(Prod.Photo.FileName).ToLowerInvariant();


                //chech 
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file extension.");
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Prod.Photo.FileName);
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images"); // Path to uploads folder
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Prod.Photo.CopyTo(stream);
                }

                var imageUrl = $"{scheme}://{host}/images/{fileName}";
                product.Photo = fileName;

            
            var NewProd=await _ProductServies.AddProduct(product);
            if(NewProd == null) {
                return BadRequest("Please check your data");
            }
            return Ok(NewProd);
        }

        [HttpGet]
        public async Task<IActionResult> GetProdByID(int  id)
        {
            var prod = await _ProductServies.GetProduct(id);
            if(prod == null)
            {
                return NotFound();
            }
            return Ok(prod);
        }


        [HttpGet]
        public async Task<IActionResult> GetProdByName(string name)
        {
            var prods=await _ProductServies.GetAllProductsByName(name);
            if (prods == null)
            {
                return NotFound();
            }
            return Ok(prods);
        }


        [HttpPut]
        public async  Task<IActionResult> UpdateProduct (int id ,[FromForm] AddProdDTO Prod)
        {
            var product = await _ProductServies.GetProduct(id);



            var scheme = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host;
            string fileExtension = Path.GetExtension(Prod.Photo.FileName).ToLowerInvariant();


            //chech 
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Invalid file extension.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Prod.Photo.FileName);
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images"); // Path to uploads folder
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Prod.Photo.CopyTo(stream);
            }

            var imageUrl = $"{scheme}://{host}/images/{fileName}";
            product.Photo = fileName;
            product.Name = Prod.Name;
            product.Brand=Prod.Brand;
            product.Description = Prod.Description;
            product.Available_Quan=Prod.Available_Quan;
            product.Price= Prod.Price;

            var UpdateProduct = _ProductServies.UpdateProduct(product);
            if (product == null)
            {
                return BadRequest();
            }
            return Ok(UpdateProduct);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _ProductServies.GetProduct(id);
            _ProductServies.DeleteProd(product);
            return Ok("Success Delete Product");
        }
    }
}
