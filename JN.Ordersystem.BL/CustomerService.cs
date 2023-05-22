using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;

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
        public List<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        /// <summary>
        /// Get the info from a specific customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The info of a specific customer</returns>
        public Customer? GetById(int id)
        {
            return _context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();
        }

        /// <summary>
        /// Create a new customer and add to the list
        /// </summary>
        /// <param name="c"></param>
        /// <returns>A newly created customer</returns>
        public Customer Create(Customer c)
        {
            _context.Customers.Add(c);
            _context.SaveChanges();

            return c;
        }

        /// <summary>
        /// Update the entire info of a specific customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="c"></param>
        /// <returns>All the updated info of a customer</returns>
        public Customer Update(int id, Customer c)
        {
            // Find the customer
            var customerToUpdate = _context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();

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

                _context.Update(customerToUpdate);
                _context.SaveChanges();

                return customerToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public bool Delete(int id)
        {
            // Find the customer
            var customer = _context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();

            // If the customer is found
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
                return true;
            }

            return false;
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
    }
}
