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
        /// <param name="productId"></param>
        /// <returns>A specific product</returns>
        public async Task<Product?> GetById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        /// <summary>
        /// Create a new product and add to the list
        /// </summary>
        /// <param name="product"></param>
        /// <returns>A newly created product</returns>
        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        /// <summary>
        /// Update the entire product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="product"></param>
        /// <returns>An updated orderDetail</returns>
        public async Task<Product?> Update(int productId, Product product)
        {
            // Find the product
            var productToUpdate = await _context.Products.FindAsync(productId);

            // If the product is found
            if (productToUpdate != null)
            {
                // Fill the properties
                productToUpdate.Description = product.Description;
                productToUpdate.ItemName = product.ItemName;
                productToUpdate.Price = product.Price;
                productToUpdate.UnitsInStock = product.UnitsInStock;

                _context.Update(productToUpdate);
                await _context.SaveChangesAsync();

                return productToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int productId)
        {
            // Find the product
            var product = await _context.Products.FindAsync(productId);

            // If the product is found
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the inventory of a product with the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="unitsInStock">The new units in stock for the product.</param>
        /// <returns>The updated product with the new inventory information, or null if the product was not found.</returns>
        public Product UpdateInventory(int productId, int unitsInStock)
        {
            // Find the product to update by its product ID
            var productToUpdate = _context.Products.Where(p => p.ProductID == productId).FirstOrDefault();

            if (productToUpdate != null)
            {
                // Update the units in stock
                productToUpdate.UnitsInStock = unitsInStock;

                // Save the changes to the database
                _context.Update(productToUpdate);
                _context.SaveChanges();

                // Return the updated product
                return productToUpdate;
            }

            // If the product was not found, return null
            return null;
        }
    }
}