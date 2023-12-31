﻿using System.ComponentModel.DataAnnotations;

namespace JN.Ordersystem.API.Models
{
    public class OrderDetail_archived
    {
        public int OrderDetailID { get; set; }

        [Required(ErrorMessage = "Please provide an order ID")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Please provide a product ID")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please provide a quantity of the chosen product")]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
