using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;

namespace JN.Ordersystem.BL
{
    public class OrderDetailService : IService<OrderDetail>
    {
        DataContext _context;

        public OrderDetailService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the orderDetails
        /// </summary>
        /// <returns>A list with all the orderDetails</returns>
        public List<OrderDetail> GetAll()
        {
            return _context.OrderDetails.ToList();
        }

        /// <summary>
        /// Get the specific orderDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific orderDetail</returns>
        public OrderDetail GetById(int id)
        {
            return _context.OrderDetails.Where(o => o.OrderDetailID == id).FirstOrDefault();
        }

        /// <summary>
        /// Create a new orderDetail and add to the list
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>A newly created orderDetail</returns>
        public OrderDetail Create(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();

            return orderDetail;
        }

        /// <summary>
        /// Update the entire orderDetail
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <param name="orderDetail"></param>
        /// <returns>An updated orderDetail</returns>
        public OrderDetail Update(int orderDetailId, OrderDetail orderDetail)
        {
            // Find the orderDetail
            var orderDetailToUpdate = _context.OrderDetails.Where(o => o.OrderDetailID == orderDetailId).FirstOrDefault();

            // If the orderDetail is found
            if (orderDetailToUpdate != null)
            {
                // Fill the properties
                orderDetailToUpdate.OrderID = orderDetail.OrderID;
                orderDetailToUpdate.ProductID = orderDetail.ProductID;
                orderDetailToUpdate.Quantity = orderDetail.Quantity;
                orderDetailToUpdate.UnitPrice = orderDetail.UnitPrice;

                _context.Update(orderDetailToUpdate);
                _context.SaveChanges();

                return orderDetailToUpdate;
            }

            return null;
        }

        /// <summary>
        /// Deletes a specific orderDetail
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns>A boolean if the delete was successful</returns>
        public bool Delete(int orderDetailId)
        {
            // Find the orderDetail
            var orderDetailToDelete = _context.OrderDetails.Find(orderDetailId);

            // If the orderDetail is found
            if (orderDetailToDelete != null)
            {
                _context.OrderDetails.Remove(orderDetailToDelete);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
