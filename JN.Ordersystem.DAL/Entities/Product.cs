using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JN.Ordersystem.DAL.Entities
{
    [Table ("Product")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string ItemName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int UnitsInStock { get; set; }
    }
}
