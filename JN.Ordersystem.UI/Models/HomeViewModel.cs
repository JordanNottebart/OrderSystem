﻿using JN.Ordersystem.DAL.Entities;

namespace JN.Ordersystem.UI.Models
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }

        public List<Customer> Customers { get; set; }

        public List<Order> Orders { get; set; }

        public Order Order { get; set; }

        public Customer MostProfitableCustomer { get; set; }

        public decimal MaxSales { get; set; }
    }
}
