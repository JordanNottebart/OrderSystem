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
        /// <summary>
        /// Get all the orders
        /// </summary>
        /// <returns>A list with all the orders</returns>
        public async Task<List<Order>> GetAll()
        {
            using (var context = new DataContext())
            {
                return await context.Orders.Include(o => o.Customer).Include(o => o.OrderDetail).ThenInclude(od => od.Product).ToListAsync();
            }    
        }

        /// <summary>
        /// Get the specific order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific order</returns>
        public async Task<Order?> GetById(int id)
        {
            using (var context = new DataContext())
            {
                return await context.Orders.FindAsync(id);
            }
        }

        /// <summary>
        /// Create a new order and add to the list
        /// </summary>
        /// <param name="order"></param>
        /// <returns>A newly created order</returns>
        public async Task<Order> Create(Order order)
        {
            using (var context = new DataContext())
            {
                context.Orders.Add(order);
                await context.SaveChangesAsync();

                return order;
            }
        }

        /// <summary>
        /// Update the entire order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns>An updated order</returns>
        public async Task<Order?> Update(int id, Order order)
        {
            using (var context = new DataContext())
            {
                // Find the order
                var orderToUpdate = await context.Orders.FindAsync(id);

                // If the order is found
                if (orderToUpdate != null)
                {
                    // Fill the properties
                    orderToUpdate.OrderDate = order.OrderDate;
                    orderToUpdate.CustomerID = order.CustomerID;
                    orderToUpdate.Status = order.Status;

                    context.Update(orderToUpdate);
                    await context.SaveChangesAsync();

                    return orderToUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Deletes a specific order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int id)
        {
            using (var context = new DataContext())
            {
                // Find the order
                var orderToDelete = await context.Orders.FindAsync(id);

                // If the order is found
                if (orderToDelete != null)
                {
                    context.Orders.Remove(orderToDelete);
                    await context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the last ID of the list of orders
        /// </summary>
        /// <returns>The last ID</returns>
        public async Task<int> GetLastId()
        {
            using (var context = new DataContext())
            {
                var lastOrder = await context.Orders
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
}
