using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JN.Ordersystem.BL
{
    public class AbstractOrderService : BaseService<Order>
    {
        public AbstractOrderService(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all the orders with the customers, orderDetails and products included
        /// </summary>
        /// <returns>A list with all the orders and with the customers, orderDetails and products included</returns>
        public override async Task<List<Order>> GetAll()
        {
            return await _context.Orders.Include(o => o.Customer).Include(o => o.OrderDetail).ThenInclude(od => od.Product).ToListAsync();
        }

        /// <summary>
        /// Deletes the Order and all the OrderDetails associated with it
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns>A boolean if the delete was successful or not</returns>
        public override async Task<bool> Delete(int OrderID)
        {
            // Retrieve the order with the specified ID
            var orderToDelete = await GetById(OrderID);

            // If the order was found
            if (orderToDelete != null)
            {
                // Retrieve all OrderDetails associated with the Order
                var orderDetailsToDelete = orderToDelete.OrderDetail;

                // Go through the list of OrderDetails
                foreach (var orderDetail in orderDetailsToDelete.ToList())   // Create a copy of the collection using ToList()
                {
                    // Delete every OrderDetail and save it in the database
                    _context.OrderDetails.Remove(orderDetail);
                    await _context.SaveChangesAsync();
                }

                // After the OrderDetails were deleted, now delete the order itself
                _context.Orders.Remove(orderToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the specific order with the customer, orderDetails and products included
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>A specific order with the customer, orderDetails and products included</returns>
        public override async Task<Order?> GetById(int orderId)
        {
            return await _context.Orders.Include(o => o.Customer).Include(o => o.OrderDetail).ThenInclude(od => od.Product).FirstOrDefaultAsync(o => o.OrderID == orderId);
        }
    }
}
