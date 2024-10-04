using EcomMakeUp.Models;
using EcomMakeUp.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Claims;

namespace EcomMakeUp.Controllers
{
    [Route("api/[controller]/[action]")]
    
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderServies _orderServies;
        private readonly IAuthServies _authServies;

        public OrderController(IOrderServies orderServies, IAuthServies authServies)
        {
            _orderServies = orderServies;
            _authServies = authServies;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder ()
        {
           // var userEmail = "Mariammaher@gmail.com";
            var userEmail= User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail == null)
            {
                return BadRequest("Error");
            }
            var user =await _authServies.SelectUserByEmail(userEmail);
            if(user == null)
            {
                return BadRequest("Error");
            }

            var order = new Order { UserId = user.Id };
            var res=await _orderServies.CreateOrder(order);
            if(res == null)
            {
                return BadRequest("Error in Create Order");
            }

            return Ok( res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders ()
        {
            var orders=await _orderServies.GetAllOrders();
            return Ok(orders);
        }

       // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetWatingOrder ()
        {
            //var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var email = "Mariammaher@gmail.com";
            var user =await _authServies.SelectUserByEmail (email);
            if( user == null )
            {
                return BadRequest("Error");
            }

            var orders=await _orderServies.GetWaitingOrdersByIdUser(user.Id);
            return Ok(orders);
        }

       
    }
}
