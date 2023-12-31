﻿using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.BL
{
    public class ArchivedOrderDetailService : IService<OrderDetail>
    {
        DataContext _context;

        public ArchivedOrderDetailService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the orderDetails
        /// </summary>
        /// <returns>A list with all the orderDetails</returns>
        public async Task<List<OrderDetail>> GetAll()
        {
            return await _context.OrderDetails.Include(od => od.Product).ToListAsync();
        }

        /// <summary>
        /// Get the specific orderDetail
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns>A specific orderDetail</returns>
        public async Task<OrderDetail?> GetById(int orderDetailId)
        {
            return await _context.OrderDetails.FindAsync(orderDetailId);
        }

        /// <summary>
        /// Create a new orderDetail and add to the list
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>A newly created orderDetail</returns>
        public async Task<OrderDetail> Create(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return orderDetail;
        }

        /// <summary>
        /// Update the entire orderDetail
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <param name="orderDetail"></param>
        /// <returns>An updated orderDetail</returns>
        public async Task<OrderDetail?> Update(int orderDetailId, OrderDetail orderDetail)
        {
            // Find the orderDetail
            var orderDetailToUpdate = await _context.OrderDetails.FindAsync(orderDetailId);

            // If the orderDetail is found
            if (orderDetailToUpdate != null)
            {
                // Fill the properties
                orderDetailToUpdate.OrderID = orderDetail.OrderID;
                orderDetailToUpdate.ProductID = orderDetail.ProductID;
                orderDetailToUpdate.Quantity = orderDetail.Quantity;

                _context.Update(orderDetailToUpdate);
                await _context.SaveChangesAsync();

                return orderDetailToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific orderDetail
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int orderDetailId)
        {
            // Find the orderDetail
            var orderDetailToDelete = await _context.OrderDetails.FindAsync(orderDetailId);

            // If the orderDetail is found
            if (orderDetailToDelete != null)
            {
                _context.OrderDetails.Remove(orderDetailToDelete);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all the orderDetails associated with a specific order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve the details for.</param>
        /// <returns>A list of order details for the specified order.</returns>
        public async Task<List<OrderDetail>> GetAllOrderDetailsByOrderId(int orderId)
        {
            return await _context.OrderDetails.Where(od => od.OrderID == orderId).ToListAsync();
        }
    }
}
