using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Models
{
    public class Customer_archived
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Please provide a last name for the customer")]
        public string CustomerLastName { get; set; }

        [Required(ErrorMessage = "Please provide a first name for the customer")]
        public string CustomerFirstName { get; set; }

        [Required(ErrorMessage = "Please provide an address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please provide a city")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please provide a postal code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please provide an email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a phone number")]
        public string Phone { get; set; }
    }
}
