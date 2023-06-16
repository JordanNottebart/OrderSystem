using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JN.Ordersystem.BL
{
    public class ProductServiceAbstract : BaseService<Product>
    {
        public ProductServiceAbstract(DataContext context) : base(context)
        {
        }

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
