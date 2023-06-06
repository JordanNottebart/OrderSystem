using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.UI.Models
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerID { get; set; }

        public SelectList Customers { get; set; }

        public List<OrderDetailViewModel> OrderDetails { get; set; }

        public string Status { get; set; }

        // Navigation Properties
        public Customer Customer { get; set; }
    }
}
