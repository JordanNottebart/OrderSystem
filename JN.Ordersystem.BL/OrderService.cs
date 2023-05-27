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
    public class OrderService : IService<Order>
    {
        DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the orders
        /// </summary>
        /// <returns>A list with all the orders</returns>
        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders.Include(o => o.Customer).ToListAsync();
        }

        /// <summary>
        /// Get the specific order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific order</returns>
        public async Task<Order?> GetById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        /// <summary>
        /// Create a new order and add to the list
        /// </summary>
        /// <param name="order"></param>
        /// <returns>A newly created order</returns>
        public async Task<Order> Create(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        /// <summary>
        /// Update the entire order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns>An updated order</returns>
        public async Task<Order?> Update(int id, Order order)
        {
            // Find the order
            var orderToUpdate = await _context.Orders.FindAsync(id);

            // If the order is found
            if (orderToUpdate != null)
            {
                // Fill the properties
                orderToUpdate.CustomerID = order.CustomerID;
                orderToUpdate.Quantity = order.Quantity;
                orderToUpdate.Status = order.Status;

                _context.Update(orderToUpdate);
                await _context.SaveChangesAsync();

                return orderToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int id)
        {
            // Find the order
            var orderToDelete = await _context.Orders.FindAsync(id);

            // If the order is found
            if (orderToDelete != null)
            {
                _context.Orders.Remove(orderToDelete);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the last ID of the list of orders
        /// </summary>
        /// <returns>The last ID</returns>
        public async Task<int> GetLastId()
        {
            var lastOrder = await _context.Orders
                .OrderByDescending(o => o.OrderID)
                .FirstOrDefaultAsync();

            if (lastOrder != null)
            {
                return lastOrder.OrderID;
            }

            // Return a default value if no products exist
            return 0;
        }

    }
}
