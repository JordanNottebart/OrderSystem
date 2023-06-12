using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JN.Ordersystem.UI.Models
{
    public class OrderDetailViewModel
    {
        public int OrderDetailViewID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public SelectList Products { get; set; }

        // Navigation Property
        public Product Product { get; set; }
    }
}
