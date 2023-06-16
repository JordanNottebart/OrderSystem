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
