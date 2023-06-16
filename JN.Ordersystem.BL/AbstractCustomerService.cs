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


    }
}
