using EcomMakeUp.Dtos;
using EcomMakeUp.Models;
using EcomMakeUp.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomMakeUp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardServies _cardsevies;
        private readonly IProductSerives _prodServies;

        public CardController(ICardServies cardsevies, IProductSerives prodServies)
        {
            _cardsevies = cardsevies;
            _prodServies = prodServies;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCard( AddToCardDto prod )
        {
            var userId = User.FindFirst("uid")?.Value;
            var product = await _prodServies.GetProduct(prod.ProductId);
            if (product == null)
            {
                return NotFound(" This product not found");
            }

            if(prod.countProduct>product.Available_Quan)
            {
                return BadRequest("this count is not available");
            }
            var addtocard = new OrderProduct();
            addtocard.UserId = userId;
            addtocard.ProductId = prod.ProductId;
            addtocard.countProduct = prod.countProduct;
            addtocard.ProductPrice = prod.countProduct * product.Price;

            var res = await _cardsevies.AddtoProduct(addtocard);
            if(res==null)
            {
                return BadRequest("This product you have it");
            }
            return Ok("Succuess");

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCard ()
        {
            var userId = User.FindFirst("uid")?.Value;

            var res = await _cardsevies.GetAllProduct(userId);

            return Ok(res);

        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int prodId)
        {
            var userId = User.FindFirst("uid")?.Value;
            var prod = await _prodServies.GetProduct(prodId);
            if(prod==null)
            {
                return NotFound("This product not found");
            }
            var res =  _cardsevies.DeleteProduct(userId, prodId);
            if(string.IsNullOrEmpty(res))
            {
                return NotFound("This product not found in this card");
            }
            return Ok(res);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateCard (AddToCardDto prod)
        {
            var userId =User.FindFirst("uid")?.Value;

            var pro=await _prodServies.GetProduct(prod.ProductId);
            if(pro==null)
            {
                return NotFound("This product not found");
            }

            var res=await _cardsevies.UpdateProduct(userId, prod);
            if (res!=null)
            {
                return NotFound("This product not found in this card");
            }
            return Ok("Success");
        }

    }
}
