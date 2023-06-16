using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JN.Ordersystem.BL
{
    public class AbstractProductService : BaseService<Product>
    {
        public AbstractProductService(DataContext context) : base(context)
        {
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
