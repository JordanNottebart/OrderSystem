using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.BL
{
    public class CustomerService : IService<Customer>
    {
        DataContext _context;

        public CustomerService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the customer infos
        /// </summary>
        /// <returns>A list with all the customer infos</returns>
        public async Task<List<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Get the info from a specific customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>The info of a specific customer</returns>
        public async Task<Customer?> GetById(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        /// <summary>
        /// Create a new customer and add to the list
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>A newly created customer</returns>
        public async Task<Customer> Create(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        /// <summary>
        /// Update the entire info of a specific customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <returns>All the updated info of a customer</returns>
        public async Task<Customer?> Update(int customerId, Customer customer)
        {
            // Find the customer
            var customerToUpdate = await _context.Customers.FindAsync(customerId);

            // If the customer is found
            if (customerToUpdate != null)
            {
                // Fill the properties
                customerToUpdate.CustomerLastName = customer.CustomerLastName;
                customerToUpdate.CustomerFirstName = customer.CustomerFirstName;
                customerToUpdate.Address = customer.Address;
                customerToUpdate.City = customer.City;
                customerToUpdate.PostalCode = customer.PostalCode;
                customerToUpdate.Email = customer.Email;
                customerToUpdate.Phone = customer.Phone;

                _context.Update(customerToUpdate);
                await _context.SaveChangesAsync();

                return customerToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int customerId)
        {
            // Find the customer
            var customer = await _context.Customers.FindAsync(customerId);

            // If the customer is found
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the full address information of a specific customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public Customer UpdateAddress(int customerId, Customer customer)
        {
            // Find the customer to update
            var customerToUpdate = _context.Customers.Where(c => c.CustomerID == customerId).FirstOrDefault();

            // If the customer exists
            if (customerToUpdate != null)
            {
                // Update the full address info
                customerToUpdate.Address = customer.Address;
                customerToUpdate.City = customer.City;
                customerToUpdate.PostalCode = customer.PostalCode;

                _context.Update(customerToUpdate);
                _context.SaveChanges();

                return customerToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Gets the last ID of the list of products
        /// </summary>
        /// <returns>The last ID</returns>
        public async Task<int> GetLastId()
        {
            var lastCustomer = await _context.Customers
                .OrderByDescending(c => c.CustomerID)
                .FirstOrDefaultAsync();

            if (lastCustomer != null)
            {
                return lastCustomer.CustomerID;
            }

            // Return a default value if no products exist
            return 0;
        }
    }
}
