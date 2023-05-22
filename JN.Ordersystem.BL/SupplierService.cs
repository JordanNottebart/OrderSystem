using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;

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
        public List<Supplier> GetAll()
        {
            return _context.Suppliers.ToList();
        }

        /// <summary>
        /// Get the info from a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>The info of a specific supplier</returns>
        public Supplier GetById(int supplierId)
        {
            return _context.Suppliers.Where(s => s.SupplierID == supplierId).FirstOrDefault();
        }

        /// <summary>
        /// Create a new supplier and add to the list
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>A newly created supplier</returns>
        public Supplier Create(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();

            return supplier;
        }

        /// <summary>
        /// Update the entire info of a specific supplier
        /// </summary>
        /// <param name="id"></param>
        /// <param name="supplier"></param>
        /// <returns>All the updated info of a supplier</returns>
        public Supplier Update(int id, Supplier supplier)
        {
            // Find the supplier
            var supplierToUpdate = _context.Suppliers.Where(s => s.SupplierID == id).FirstOrDefault();

            // If the supplier is found
            if (supplierToUpdate != null)
            {
                // Fill the properties
                supplierToUpdate.SupplierName = supplier.SupplierName;
                supplierToUpdate.ContactInfo = supplier.ContactInfo;

                _context.Update(supplierToUpdate);
                _context.SaveChanges();

                return supplierToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public bool Delete(int supplierId)
        {
            // Find the supplier
            var supplierToDelete = _context.Suppliers.Find(supplierId);

            // If the supplier is found
            if (supplierToDelete != null)
            {
                _context.Suppliers.Remove(supplierToDelete);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
