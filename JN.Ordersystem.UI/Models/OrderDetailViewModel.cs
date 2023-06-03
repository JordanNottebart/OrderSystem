using Microsoft.AspNetCore.Mvc.Rendering;

namespace JN.Ordersystem.UI.Models
{
    public class OrderDetailViewModel
    {
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public SelectList Products { get; set; }
    }
}
