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
        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        /// <summary>
        /// Get the specific order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific order</returns>
        public Order? GetById(int id)
        {
            return _context.Orders.Where(order => order.OrderID == id).FirstOrDefault();
        }

        /// <summary>
        /// Create a new order and add to the list
        /// </summary>
        /// <param name="order"></param>
        /// <returns>A newly created order</returns>
        public Order Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        /// <summary>
        /// Update the entire order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns>An updated order</returns>
        public Order Update(int id, Order order)
        {
            // Find the order
            var orderToUpdate = _context.Orders.Where(order => order.OrderID == id).FirstOrDefault();

            // If the order is found
            if (orderToUpdate != null)
            {
                // Fill the properties
                orderToUpdate.CustomerID = order.CustomerID;
                orderToUpdate.Quantity = order.Quantity;
                orderToUpdate.Status = order.Status;

                _context.Update(orderToUpdate);
                _context.SaveChanges();

                return orderToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public bool Delete(int id)
        {
            // Find the orderDetail
            var order = _context.Orders.Where(order => order.OrderID == id).FirstOrDefault();

            // If the order is found
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
