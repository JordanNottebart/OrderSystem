using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using JN.Ordersystem.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.UI.Controllers
{
    public class OrderController : Controller
    {
        readonly OrderService _orderService;
        readonly CustomerService _customerService;
        readonly ProductService _productService;
        readonly OrderDetailService _orderDetailService;

        public OrderController(OrderService orderService, CustomerService customerService, ProductService productService, OrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
            _orderDetailService = orderDetailService;
        }

        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            return View(await _orderService.GetAll());
        }

        // GET: /Product/Create
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            List<Customer> customers = await _customerService.GetAll();
            List<Product> products = await _productService.GetAll();
            SelectList customerList = new SelectList(customers, "CustomerID", "CustomerFullName");
            SelectList productList = new SelectList(products, "ProductID", "ProductFull");

            int lastOrderId = await _orderService.GetLastId();
            DateTime now = DateTime.Now;
            DateTime dateTimeWithSecondsZero = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            var orderDetailsList = new List<OrderDetailViewModel>();

            var orderDetailOne = new OrderDetailViewModel
            {
                Products = productList
            };

            orderDetailsList.Add(orderDetailOne);

            var order = new OrderViewModel
            {
                OrderID = lastOrderId + 1,
                OrderDate = dateTimeWithSecondsZero,
                Customers = customerList,
                OrderDetails = orderDetailsList
            };

            return View(order);
        }

        // POST: /Order/Create
        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            // Create the order
            var order = new Order
            {
                OrderDate = model.OrderDate,
                CustomerID = model.CustomerID,
                Status = "Unfulfilled" // Set the initial status here
            };

            await _orderService.Create(order);

            // Create the order detail
            var orderDetail = new OrderDetail
            {
                OrderID = order.OrderID,
                ProductID = model.OrderDetails[0].ProductID,
                Quantity = model.OrderDetails[0].Quantity
            };

            await _orderDetailService.Create(orderDetail);

            return RedirectToAction("Index", "Order"); // Redirect to the order index page
        }

        // Helper method to get Customers as SelectList
        private async Task<SelectList> GetCustomersAsSelectListAsync()
        {
            // Replace with your logic to fetch customers and return them as a SelectList
            var customers = await _customerService.GetAll();
            return new SelectList(customers, "CustomerID", "CustomerFullName");
        }

        // Helper method to get Products as SelectList
        private async Task<SelectList> GetProductsAsSelectListAsync()
        {
            // Replace with your logic to fetch products and return them as a SelectList
            var products = await _productService.GetAll();
            return new SelectList(products, "ProductID", "ProductFull");
        }

        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            // Update the status in the database
            var order = await _orderService.GetById(orderId);
            order.Status = status;
            await _orderService.Update(orderId, order);

            // Fetch the updated order details
            var updatedOrder = await _orderService.GetById(orderId);

            return Json(new { success = true, data = updatedOrder });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUnitsInStock(int orderId)
        {
            try
            {
                var order = await _orderService.GetById(orderId);

                if (order == null)
                {
                    return NotFound();
                }

                foreach (var detail in order.OrderDetail)
                {
                    var product = await _productService.GetById(detail.ProductID);
                    if (product != null)
                    {
                        product.UnitsInStock -= detail.Quantity;
                        await _productService.Update(product.ProductID, product);
                    }
                }

                return RedirectToAction("Index", "Order");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
