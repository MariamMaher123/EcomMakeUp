using EcomMakeUp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcomMakeUp.Servies
{
    public class ProductSerives : IProductSerives
    {

        public readonly ApplicationDbContext _dbcontext;

        public ProductSerives(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Products> AddProduct(Products product)
        {
            await _dbcontext.AddAsync(product);
            await _dbcontext.SaveChangesAsync();
            return product;
        }

        public Products DeleteProd(Products products)
        {
            _dbcontext.Remove(products);
            _dbcontext.SaveChanges();
            return products;
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            var Prods = await _dbcontext.Products.ToListAsync();
            return Prods;
        }

        public async Task<IEnumerable<Products>> GetAllProductsByName(string productName)
        {
            var prods= await _dbcontext.Products.Where(x=>x.Name.Contains(productName)).ToListAsync(); 
            return prods;   
        }

        public async Task<Products> GetProduct(int productId)
        {
            var prod =await _dbcontext.Products.FirstOrDefaultAsync(x=>x.Id==productId);
            return prod;
        }

        public Products UpdateProduct(Products product)
        {
            _dbcontext.Update(product);
            _dbcontext.SaveChanges();
            return product;
        }

    }
}
