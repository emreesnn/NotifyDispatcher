using Microsoft.EntityFrameworkCore;
using NotifyDispatcher.Models;

namespace NotifyDispatcher.Data
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAll() 
        { 
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetById(int productId) 
        {
            return await _context.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();
        }
        public async Task<Product> GetByUrl(string productUrl) 
        {
            return await _context.Products
                    .Where(p => p.ProductUrl == productUrl)
                    .FirstOrDefaultAsync();
        }
        public async Task<bool> CreateProductAsync(Product product) 
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateProductPriceAsync(int id,String price) 
        { 
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.LastKnownPrice = price;

            await _context.SaveChangesAsync();  
            return true;
        }
        public async Task<bool> DeleteProductAsync(int id) 
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
