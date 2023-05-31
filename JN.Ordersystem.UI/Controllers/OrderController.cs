using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JN.Ordersystem.UI.Controllers
{
    public class OrderController : Controller
    {
        OrderService _orderService;
        CustomerService _customerService;
        ProductService _productService;

        public OrderController(OrderService service, CustomerService customerService, ProductService productService)
        {
            _orderService = service;
            _customerService = customerService;
            _productService = productService;
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
            SelectList customerList = new SelectList(customers, "CustomerID", "CustomerFullName");
            ViewBag.Customers = customerList;

            int lastOrderId = await _orderService.GetLastId();
            DateTime now = DateTime.Now;
            DateTime dateTimeWithSecondsZero = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            var order = new Order
            {
                OrderID = lastOrderId + 1,
                OrderDate = dateTimeWithSecondsZero,
                Status = "Unfulfilled"
            };

            return View(order);
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
