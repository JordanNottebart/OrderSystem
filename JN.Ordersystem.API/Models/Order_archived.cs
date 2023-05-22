using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Models
{
    public class Order_archived
    {
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Please provide a customer ID")]
        public int CustomerID { get; set; }

        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please provide a status")]
        public string Status { get; set; }
    }
}
