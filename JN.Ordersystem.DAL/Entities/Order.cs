using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JN.Ordersystem.DAL.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerID { get; set; }

        public string Status { get; set; }

        // Navigation Properties
        public Customer Customer { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }

    }
}
