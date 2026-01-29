using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre.Data
{
    public class ProductRepository(StoreContext _context) : IProductRepository
    {

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetBrandAsync()
        {
            return await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;

        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            var query = _context.Products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(u => u.Brand == brand);
            }
            if(!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(u => u.Type == type);
            }

            query = sort switch
            {
                "priceAsc" => query.OrderBy(u => u.Price),
                "priceDesc" => query.OrderByDescending(u => u.Price),
                _ => query.OrderBy(u => u.Name)
            };
            return await query.ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(u => u.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
