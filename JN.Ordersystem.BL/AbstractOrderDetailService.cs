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
    public class AbstractOrderDetailService : BaseService<OrderDetail>
    {
        public AbstractOrderDetailService(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all the orderDetails including the products
        /// </summary>
        /// <returns>A list with all the orderDetails including the products</returns>
        public override async Task<List<OrderDetail>> GetAll()
        {
            return await _context.OrderDetails.Include(od => od.Product).ToListAsync();
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
