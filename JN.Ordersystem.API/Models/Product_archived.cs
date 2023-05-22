using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Models
{
    public class Product_archived
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please provide a name")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Please provide a description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please provide a price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please provide the units in stock")]
        public int UnitsInStock { get; set; }
    }
}
