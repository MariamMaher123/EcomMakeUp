using EcomMakeUp.Models;

namespace EcomMakeUp.Servies
{
    public interface IProductSerives
    {
        public Task<Products> AddProduct (Products product);
        public Products UpdateProduct (Products product);
        public Products DeleteProd(Products products);
        public Task<IEnumerable< Products>> GetAllProducts ();
        public Task<IEnumerable<Products>> GetAllProductsByName (string productName);
        public Task<Products> GetProduct (int  productId);

    }
}
