using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.BL
{
    public class SupplierService : IService<Supplier>
    {
        DataContext _context;

        public SupplierService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the supplier infos
        /// </summary>
        /// <returns>A list with all the supplier infos</returns>
        public async Task<List<Supplier>> GetAll()
        {
            return await _context.Suppliers.ToListAsync();
        }

        /// <summary>
        /// Get the info from a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>The info of a specific supplier</returns>
        public async Task<Supplier?> GetById(int supplierId)
        {
            return await _context.Suppliers.FindAsync(supplierId);
        }

        /// <summary>
        /// Create a new supplier and add to the list
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>A newly created supplier</returns>
        public async Task<Supplier> Create(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        /// <summary>
        /// Update the entire info of a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="supplier"></param>
        /// <returns>All the updated info of a supplier</returns>
        public async Task<Supplier?> Update(int supplierId, Supplier supplier)
        {
            // Find the supplier
            var supplierToUpdate = await _context.Suppliers.FindAsync(supplierId);

            // If the supplier is found
            if (supplierToUpdate != null)
            {
                // Update the properties
                supplierToUpdate.SupplierName = supplier.SupplierName;
                supplierToUpdate.ContactInfo = supplier.ContactInfo;

                await _context.SaveChangesAsync();

                return supplierToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int supplierId)
        {
            // Find the supplier
            var supplierToDelete = await _context.Suppliers.FindAsync(supplierId);

            // If the supplier is found
            if (supplierToDelete != null)
            {
                _context.Suppliers.Remove(supplierToDelete);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
