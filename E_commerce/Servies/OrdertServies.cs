using EcomMakeUp.Dtos;
using EcomMakeUp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcomMakeUp.Servies
{
    public class OrdertServies : IOrderServies
    {

        private readonly ApplicationDbContext _context;

        public OrdertServies(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> CreateOrder(Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            return new OrderModelDto { date=order.OrderDate , status=order.Status 
            , OrderId = order.Id , UserId=order.UserId , UserName=order.User.FullName};
        }

        public string  DeleteOrder(Order order)
        {
           var res= _context.Remove(order);
            _context.SaveChanges();
            return ("Success delete ");

        }

        public async Task<IEnumerable<object>> GetAllOrders()
        {
            var orders = await _context.Orders.Include(x => x.User).Select(z => new {z.Id , 
            z.UserId,z.User.FullName , z.Status , z.OrderDate }).ToListAsync();
            
            return orders;
            
        }

        public async Task<IEnumerable<object>> GetBuyingOrdersByIdUser(string userId)
        {
           return await _context.Orders.Where(x=>x.Status=="Buying" && x.UserId == userId).Include(x => x.User).Select(z => new
           {
               z.Id,
               z.UserId,
               z.User.FullName,
               z.Status,
               z.OrderDate
           }).ToListAsync();    
        }

        public async Task<object> GetOrderById(int orderId)
        {
            var ord = await _context.Orders.Where(x => x.Id == orderId).Include(x => x.User).Select(z => new { z.Id, z.UserId, z.User.FullName, z.Status, z.OrderDate }).FirstOrDefaultAsync();
            return ord;
        }

        public async Task<IEnumerable<object>> GetOrdersByIdUser(string userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).Include(x => x.User).Select(z => new
            {
                z.Id,
                z.UserId,
                z.User.FullName,
                z.Status,
                z.OrderDate
            }).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetWaitingOrdersByIdUser(string userId)
        {
            return await _context.Orders.Where(x=>x.UserId == userId && x.Status=="Waiting").Include(x => x.User).Select(z => new
            {
                z.Id,
                z.UserId,
                z.User.FullName,
                z.Status,
                z.OrderDate
            }).ToListAsync();
        }

        public async Task<object> UpdateOrder(Order order)
        {
             _context.Update(order);
             await _context.SaveChangesAsync();

            return new OrderModelDto
            {
                date = order.OrderDate,
                status = order.Status
            ,
                OrderId = order.Id,
                UserId = order.UserId,
                UserName = order.User.FullName
            };
            
        }
    }
}
