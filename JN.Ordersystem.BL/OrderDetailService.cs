using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.BL
{
    public class OrderDetailService : IService<OrderDetail>
    {
        /// <summary>
        /// Get all the orderDetails
        /// </summary>
        /// <returns>A list with all the orderDetails</returns>
        public async Task<List<OrderDetail>> GetAll()
        {
            using (var context = new DataContext())
            {
                return await context.OrderDetails.Include(od => od.Product).ToListAsync();
            }
        }

        /// <summary>
        /// Get the specific orderDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific orderDetail</returns>
        public async Task<OrderDetail?> GetById(int orderDetailId)
        {
            using (var context = new DataContext())
            {
                return await context.OrderDetails.FindAsync(orderDetailId);
            }
        }

        /// <summary>
        /// Create a new orderDetail and add to the list
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>A newly created orderDetail</returns>
        public async Task<OrderDetail> Create(OrderDetail orderDetail)
        {
            using (var context = new DataContext())
            {
                context.OrderDetails.Add(orderDetail);
                await context.SaveChangesAsync();

                return orderDetail;
            }
        }

        /// <summary>
        /// Update the entire orderDetail
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <param name="orderDetail"></param>
        /// <returns>An updated orderDetail</returns>
        public async Task<OrderDetail?> Update(int orderDetailId, OrderDetail orderDetail)
        {
            using (var context = new DataContext())
            {
                // Find the orderDetail
                var orderDetailToUpdate = await context.OrderDetails.FindAsync(orderDetailId);

                // If the orderDetail is found
                if (orderDetailToUpdate != null)
                {
                    // Fill the properties
                    orderDetailToUpdate.OrderID = orderDetail.OrderID;
                    orderDetailToUpdate.ProductID = orderDetail.ProductID;
                    orderDetailToUpdate.Quantity = orderDetail.Quantity;

                    context.Update(orderDetailToUpdate);
                    await context.SaveChangesAsync();

                    return orderDetailToUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Deletes a specific orderDetail
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public async Task<bool> Delete(int orderDetailId)
        {
            using (var context = new DataContext())
            {
                // Find the orderDetail
                var orderDetailToDelete = await context.OrderDetails.FindAsync(orderDetailId);

                // If the orderDetail is found
                if (orderDetailToDelete != null)
                {
                    context.OrderDetails.Remove(orderDetailToDelete);
                    await context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the last ID of the list of orderDetails
        /// </summary>
        /// <returns>The last ID</returns>
        public async Task<int> GetLastId()
        {
            using (var context = new DataContext())
            {
                var lastOrderDetail = await context.OrderDetails
                    .OrderByDescending(o => o.OrderDetailID)
                    .FirstOrDefaultAsync();

                if (lastOrderDetail != null)
                {
                    return lastOrderDetail.OrderDetailID;
                }

                // Return a default value if no products exist
                return 0;
            }
        }
    }
}
