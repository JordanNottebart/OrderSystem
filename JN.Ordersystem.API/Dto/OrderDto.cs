using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Dto
{
    public class OrderDto
    {
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Please provide a customer ID")]
        public int CustomerID { get; set; }

        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please provide a status")]
        public string Status { get; set; }
    }
}
