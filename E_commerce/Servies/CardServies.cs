using EcomMakeUp.Dtos;
using EcomMakeUp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcomMakeUp.Servies
{
    public class CardServies : ICardServies
    {

        private readonly ApplicationDbContext _dbcontext;
        private readonly IProductSerives _productSerives;

        public CardServies(ApplicationDbContext dbcontext, IProductSerives productSerives)
        {
            _dbcontext = dbcontext;
            _productSerives = productSerives;
        }

        public async Task<object> AddtoProduct(OrderProduct product)
        {
            var pro = await _dbcontext.Card.FirstOrDefaultAsync(x=> x.ProductId == product.ProductId && x.UserId==product.UserId);
         var product1 =await _productSerives.GetProduct(product.ProductId);
            if(pro != null || product1==null) {
                return (null);
            }

            await _dbcontext.AddAsync(product);
            await _dbcontext.SaveChangesAsync();

            return ("Succies");

        }

        public string DeleteProduct(string userId, int productId)
        {
            var prod= _dbcontext.Card.FirstOrDefault(x=>x.ProductId==productId && x.UserId==userId);
            if (prod == null)
            {
                return (null);
            }

            _dbcontext.Remove(prod);
            _dbcontext.SaveChanges();
            return ("Success Delete");

        }

        public Task<decimal> GetAllPriceInCard(string UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetAllProduct(string UserId)
        {
            var prodList = await _dbcontext.Card
        .Where(x => x.UserId == UserId)
        .Include(x => x.Productcs)
        .Select(x => new
        {
            x.ProductId,
            ProductName = x.Productcs.Name,
            x.ProductPrice,
            x.Productcs.Photo,
            x.countProduct 
        })
        .ToListAsync();

            return (prodList);
        }

        public async Task<object> UpdateProduct(string userId, AddToCardDto card)
        {
            var pro=await _dbcontext.Card.FirstOrDefaultAsync(x=>x.UserId==userId && x.ProductId==card.ProductId);
            if(pro == null)
            {
                return ("Not found");
            }
            var product = await _productSerives.GetProduct(card.ProductId);
            if (product == null)
            {
                return ("Not found");
            }

            pro.countProduct=card.countProduct;
            pro.ProductPrice = card.countProduct * product.Price;
            pro.DateAdd = DateTime.Now;

             _dbcontext.Update(pro);
            _dbcontext.SaveChangesAsync();
            return (null);
        }
    }
}
