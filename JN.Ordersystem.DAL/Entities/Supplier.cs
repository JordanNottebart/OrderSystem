using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JN.Ordersystem.DAL.Entities
{
    [Table ("Suppliers")]
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        public string SupplierName { get; set; }

        public string ContactInfo { get; set;}
    }
}
