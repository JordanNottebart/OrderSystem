using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JN.Ordersystem.DAL.Entities
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [Required(ErrorMessage = "Please select a customer.")]
        public int CustomerID { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerFirstName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string CustomerFullName
        {
            get { return $"{CustomerID}. {CustomerLastName} {CustomerFirstName}" ; }
        }

    }
}
