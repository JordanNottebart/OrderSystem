using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Dto
{
    public class SupplierDto
    {
        [Required(ErrorMessage = "Please provide a supplier name")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Please provide the contact info of the supplier")]
        public string ContactInfo { get; set; }
    }
}
