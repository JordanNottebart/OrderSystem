using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Models
{
    public class Supplier_archived
    {
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Please provide a supplier name")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Please provide the contact info of the supplier")]
        public string ContactInfo { get; set; }
    }
}
