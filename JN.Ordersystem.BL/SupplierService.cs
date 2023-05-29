using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.BL
{
    public class SupplierService : IService<Supplier>
    {
        /// <summary>
        /// Get all the supplier infos
        /// </summary>
        /// <returns>A list with all the supplier infos</returns>
        public async Task<List<Supplier>> GetAll()
        {
            using (var context = new DataContext())
            {
                return await context.Suppliers.ToListAsync();
            }    
        }

        /// <summary>
        /// Get the info from a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>The info of a specific supplier</returns>
        public async Task<Supplier?> GetById(int supplierId)
        {
            using (var context = new DataContext())
            {
                return await context.Suppliers.FindAsync(supplierId);
            }
        }

        /// <summary>
        /// Create a new supplier and add to the list
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>A newly created supplier</returns>
        public async Task<Supplier> Create(Supplier supplier)
        {
            using (var context = new DataContext())
            {
                context.Suppliers.Add(supplier);
                await context.SaveChangesAsync();

                return supplier;
            }
        }

        /// <summary>
        /// Update the entire info of a specific supplier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="supplier"></param>
        /// <returns>All the updated info of a supplier</returns>
        public async Task<Supplier?> Update(int id, Supplier supplier)
        {
            using (var context = new DataContext())
            {
                // Find the supplier
                var supplierToUpdate = await context.Suppliers.FindAsync(id);

                // If the supplier is found
                if (supplierToUpdate != null)
                {
                    // Update the properties
                    supplierToUpdate.SupplierName = supplier.SupplierName;
                    supplierToUpdate.ContactInfo = supplier.ContactInfo;

                    await context.SaveChangesAsync();

                    return supplierToUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Deletes a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int supplierId)
        {
            using (var context = new DataContext())
            {
                // Find the supplier
                var supplierToDelete = await context.Suppliers.FindAsync(supplierId);

                // If the supplier is found
                if (supplierToDelete != null)
                {
                    context.Suppliers.Remove(supplierToDelete);
                    await context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the last ID of the list of suppliers
        /// </summary>
        /// <returns>The last ID</returns>
        public async Task<int> GetLastId()
        {
            using (var context = new DataContext())
            {
                var lastSupplier = await context.Suppliers
                .OrderByDescending(s => s.SupplierID)
                .FirstOrDefaultAsync();

                if (lastSupplier != null)
                {
                    return lastSupplier.SupplierID;
                }

                // Return a default value if no products exist
                return 0;
            }
        }
    }
}
