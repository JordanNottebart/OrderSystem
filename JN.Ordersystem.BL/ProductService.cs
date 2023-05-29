using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace JN.Ordersystem.BL
{

    public class ProductService : IService<Product>
    {
        /// <summary>
        /// Get all the products
        /// </summary>
        /// <returns>A list with all the products</returns>
        public async Task<List<Product>> GetAll()
        {
            using (var context = new DataContext())
            {
                return await context.Products.ToListAsync();

            }
        }

        /// <summary>
        /// Get the specific product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific product</returns>
        public async Task<Product?> GetById(int id)
        {
            using (var context = new DataContext())
            {
                return await context.Products.FindAsync(id);
            }
        }

        /// <summary>
        /// Create a new product and add to the list
        /// </summary>
        /// <param name="p"></param>
        /// <returns>A newly created product</returns>
        public async Task<Product> Create(Product p)
        {
            using (var context = new DataContext())
            {
                context.Products.Add(p);
                await context.SaveChangesAsync();

                return p;
            }
        }

        /// <summary>
        /// Update the entire product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns>An updated orderDetail</returns>
        public async Task<Product?> Update(int id, Product p)
        {
            using (var context = new DataContext())
            {
                // Find the product
                var productToUpdate = await context.Products.FindAsync(id);

                // If the product is found
                if (productToUpdate != null)
                {
                    // Fill the properties
                    productToUpdate.Description = p.Description;
                    productToUpdate.ItemName = p.ItemName;
                    productToUpdate.Price = p.Price;
                    productToUpdate.UnitsInStock = p.UnitsInStock;

                    context.Update(productToUpdate);
                    await context.SaveChangesAsync();

                    return productToUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Deletes a specific product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int id)
        {
            using (var context = new DataContext())
            {
                // Find the product
                var product = await context.Products.FindAsync(id);

                // If the product is found
                if (product != null)
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }

        public Product UpdateInventory(int id, int unitsInStock)
        {
            using (var context = new DataContext())
            {
                var productToUpdate = context.Products.Where(p => p.ProductID == id).FirstOrDefault();

                if (productToUpdate != null)
                {
                    productToUpdate.UnitsInStock = unitsInStock;

                    context.Update(productToUpdate);
                    context.SaveChanges();

                    return productToUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the last ID of the list of products
        /// </summary>
        /// <returns>The last ID</returns>
        public async Task<int> GetLastId()
        {
            using (var context = new DataContext())
            {
                var lastProduct = await context.Products
                .OrderByDescending(p => p.ProductID)
                .FirstOrDefaultAsync();

                if (lastProduct != null)
                {
                    return lastProduct.ProductID;
                }

                // Return a default value if no products exist
                return 0;
            }
        }
    }
}