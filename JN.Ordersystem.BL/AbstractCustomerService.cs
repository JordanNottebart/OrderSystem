using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JN.Ordersystem.BL
{
    public class AbstractCustomerService : BaseService<Customer>
    {
        public AbstractCustomerService(DataContext context) : base(context)
        {
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
    }
}
