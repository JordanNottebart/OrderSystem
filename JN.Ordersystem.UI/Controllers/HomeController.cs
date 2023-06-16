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
        readonly OrderService _orderService;
        readonly AbstractCustomerService _customerService;

        public HomeController(ILogger<HomeController> logger, AbstractProductService productService, OrderService orderService, AbstractCustomerService customerService)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var listOfProducts = await _productService.GetAll();
            var listOfOrders = await _orderService.GetAll();
            var customerOrders = listOfOrders.GroupBy(o => o.Customer);

            var listOfLowProducts = listOfProducts.Where(p => p.UnitsInStock < 20).ToList();
            var mostRecentOrder = listOfOrders.OrderByDescending(o => o.OrderDate).Where(o => o.Status != "Shipped").FirstOrDefault();

            decimal maxTotalSales = 0;
            Customer mostProfitableCustomer = null;

            foreach (var customerGroup in customerOrders)
            {
                decimal totalSales = 0;

                foreach (var order in customerGroup)
                {
                    foreach (var detail in order.OrderDetail)
                    {
                        totalSales += detail.Quantity * detail.Product.Price;
                    }
                }

                if (totalSales > maxTotalSales)
                {
                    maxTotalSales = totalSales;
                    mostProfitableCustomer = customerGroup.Key;
                }
            }

            var lowProductViewModel = new HomeViewModel
            {
                Products = listOfLowProducts,
                Order = mostRecentOrder,
                MostProfitableCustomer = mostProfitableCustomer,
                MaxSales = maxTotalSales
                
            };

            return View(lowProductViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult> Resupply()
        {
            var listOfProducts = await _productService.GetAll();

            var listOfLowProducts = listOfProducts.Where(p => p.UnitsInStock <= 20);

            foreach (var product in listOfLowProducts)
            {
                product.UnitsInStock += 50;
                _productService.UpdateInventory(product.ProductID, product.UnitsInStock);
            }

            // Return a success response
            return Json(new { success = true });
        }
    }
}