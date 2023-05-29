using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.BL
{
    public class CustomerService : IService<Customer>
    {
        /// <summary>
        /// Get all the customer infos
        /// </summary>
        /// <returns>A list with all the customer infos</returns>
        public async Task<List<Customer>> GetAll()
        {
            using (var context = new DataContext())
            {
                return await context.Customers.ToListAsync();

            }
        }

        /// <summary>
        /// Get the info from a specific customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The info of a specific customer</returns>
        public async Task<Customer?> GetById(int id)
        {
            using (var context = new DataContext())
            {
                return await context.Customers.FindAsync(id);

            }
        }

        /// <summary>
        /// Create a new customer and add to the list
        /// </summary>
        /// <param name="c"></param>
        /// <returns>A newly created customer</returns>
        public async Task<Customer> Create(Customer c)
        {
            using (var context = new DataContext())
            {
                context.Customers.Add(c);
                await context.SaveChangesAsync();

                return c;
            }
        }

        /// <summary>
        /// Update the entire info of a specific customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="c"></param>
        /// <returns>All the updated info of a customer</returns>
        public async Task<Customer?> Update(int id, Customer c)
        {
            using (var context = new DataContext())
            {
                // Find the customer
                var customerToUpdate = await context.Customers.FindAsync(id);

                // If the customer is found
                if (customerToUpdate != null)
                {
                    // Fill the properties
                    customerToUpdate.CustomerLastName = c.CustomerLastName;
                    customerToUpdate.CustomerFirstName = c.CustomerFirstName;
                    customerToUpdate.Address = c.Address;
                    customerToUpdate.City = c.City;
                    customerToUpdate.PostalCode = c.PostalCode;
                    customerToUpdate.Email = c.Email;
                    customerToUpdate.Phone = c.Phone;

                    context.Update(customerToUpdate);
                    await context.SaveChangesAsync();

                    return customerToUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Deletes a specific customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int id)
        {
            using (var context = new DataContext())
            {
                // Find the customer
                var customer = await context.Customers.FindAsync(id);

                // If the customer is found
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    await context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
        }

        //public Customer Patch(int id, Customer c)
        //{
        //    var customerToUpdate = _context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();

        //    if (customerToUpdate != null)
        //    {
        //        customerToUpdate.Address = c.Address;
        //        customerToUpdate.City = c.City;
        //        customerToUpdate.PostalCode = c.PostalCode;

        //        _context.Update(customerToUpdate);
        //        _context.SaveChanges();

        //        return customerToUpdate;
        //    }

        //    return null;
        //}

        /// <summary>
        /// Gets the last ID of the list of products
        /// </summary>
        /// <returns>The last ID</returns>
        public async Task<int> GetLastId()
        {
            using (var context = new DataContext())
            {
                var lastCustomer = await context.Customers
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
}
