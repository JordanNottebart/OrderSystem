using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Dto
{
    public class ProductDto
    {
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
