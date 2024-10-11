using EcomMakeUp.Dtos;
using EcomMakeUp.Models;

namespace EcomMakeUp.Servies
{
    public interface ICardServies
    {
        Task<object> AddtoProduct(OrderProduct product);
        Task<IEnumerable<Object>> GetAllProduct( string UserId);
       
        string DeleteProduct (string userId , int productId);
        Task<object> UpdateProduct(string userId, AddToCardDto card);
        Task <decimal> GetAllPriceInCard(string UserId);

    }
}
