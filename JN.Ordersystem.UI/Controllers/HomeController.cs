using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using JN.Ordersystem.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JN.Ordersystem.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly AbstractProductService _productService;
        readonly AbstractOrderService _orderService;
        readonly AbstractCustomerService _customerService;

        public HomeController(ILogger<HomeController> logger, AbstractProductService productService, AbstractOrderService orderService, AbstractCustomerService customerService)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            _customerService = customerService;
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            // Get all the products
            var listOfProducts = await _productService.GetAll();

            // Get all the orders
            var listOfOrders = await _orderService.GetAll();

            // Group all the orders with their customer
            var customerOrders = listOfOrders.GroupBy(o => o.Customer);

            // Get all the products where the units in stock are lower than 20
            var listOfLowProducts = listOfProducts.Where(p => p.UnitsInStock < 20).ToList();

            // Order all the orders, so that the first order in the list is the most recent one by date and filter them where the status is not "Shipped"
            var mostRecentOrder = listOfOrders.OrderByDescending(o => o.OrderDate).Where(o => o.Status != "Shipped").FirstOrDefault();

            decimal maxTotalSales = 0;
            Customer? mostProfitableCustomer = null;

            // Go through the grouping of the customer orders
            foreach (var customerGroup in customerOrders)
            {
                decimal totalSales = 0;

                // Go through every order for that customer
                foreach (var order in customerGroup)
                {
                    // Check every detail of the order
                    foreach (var detail in order.OrderDetail)
                    {
                        // Calculate the total sales for that customer
                        totalSales += detail.Quantity * detail.Product.Price;
                    }
                }

                // If the total sales are higher than the current max total sales
                if (totalSales > maxTotalSales)
                {
                    // Become the new max total sales and most profitable customer
                    maxTotalSales = totalSales;
                    mostProfitableCustomer = customerGroup.Key;
                }
            }

            // Make a new homeviewmodel, to pass through to the view
            var newHomeViewModel = new HomeViewModel
            {
                Products = listOfLowProducts,
                Order = mostRecentOrder,
                MostProfitableCustomer = mostProfitableCustomer,
                MaxSales = maxTotalSales
            };

            return View(newHomeViewModel);
        }

        // Not used for now
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Method to resupply the low product in stock
        [HttpPost]
        public async Task<ActionResult> Resupply()
        {
            // Get all the products
            var listOfProducts = await _productService.GetAll();

            // Get all the products where the units in stock are lower than 20
            var listOfLowProducts = listOfProducts.Where(p => p.UnitsInStock < 20);

            // Go through the list of low products
            foreach (var product in listOfLowProducts)
            {
                // "Resupply" the product
                product.UnitsInStock += 50;

                // Update the inventory to the database
                _productService.UpdateInventory(product.ProductID, product.UnitsInStock);
            }

            // Return a success response
            return Json(new { success = true });
        }
    }
}