using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace JN.Ordersystem.BL
{

    public class ProductService : IService<Product>
    {
        DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the products
        /// </summary>
        /// <returns>A list with all the products</returns>
        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Get the specific product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific product</returns>
        public async Task<Product?> GetById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
        }

        /// <summary>
        /// Create a new product and add to the list
        /// </summary>
        /// <param name="p"></param>
        /// <returns>A newly created product</returns>
        public async Task<Product> Create(Product p)
        {
            _context.Products.Add(p);
            await _context.SaveChangesAsync();

            return p;
        }

        /// <summary>
        /// Update the entire product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns>An updated orderDetail</returns>
        public async Task<Product> Update(int id, Product p)
        {
            // Find the product
            var productToUpdate = _context.Products.Where(p => p.ProductID == id).FirstOrDefault();

            // If the product is found
            if (productToUpdate != null)
            {
                // Fill the properties
                productToUpdate.Description = p.Description;
                productToUpdate.ItemName = p.ItemName;
                productToUpdate.Price = p.Price;
                productToUpdate.UnitsInStock = p.UnitsInStock;

                _context.Update(productToUpdate);
                await _context.SaveChangesAsync();

                return productToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int id)
        {
            // Find the product
            var product = _context.Products.Where(p => p.ProductID == id).FirstOrDefault();

            // If the product is found
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public Product UpdateInventory(int id, int unitsInStock)
        {
            var productToUpdate = _context.Products.Where(p => p.ProductID == id).FirstOrDefault();

            if (productToUpdate != null)
            {
                productToUpdate.UnitsInStock = unitsInStock;

                _context.Update(productToUpdate);
                _context.SaveChanges();

                return productToUpdate;
            }

            return null;
        }
    }
}