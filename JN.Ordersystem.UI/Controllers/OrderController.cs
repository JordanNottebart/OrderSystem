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

        public async Task<ActionResult> Details(int id)
        {
            var order = await _orderService.GetById(id);
            return View(order);
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

            var initOrderDetail = new OrderDetailViewModel
            {
                Products = productList
            };

            orderDetailsList.Add(initOrderDetail);

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
        public async Task<ActionResult> Create(OrderViewModel model, List<OrderDetailViewModel> selectedProducts)
        {
            if (model.CustomerID == 0 || selectedProducts.Count == 0)
            {
                ModelState.AddModelError("", "Please select a customer and add at least one product to the cart.");
                // Retrieve the customers and products again to pass to the view
                List<Customer> customers = await _customerService.GetAll();
                List<Product> products = await _productService.GetAll();
                model.Customers = new SelectList(customers, "CustomerID", "CustomerFullName");
                model.OrderDetails[0].Products = new SelectList(products, "ProductID", "ProductFull");
                model.OrderID = await _orderService.GetLastId() + 1;
                return View(model);
            }

            // Create the order
            var order = new Order
            {
                OrderDate = model.OrderDate,
                CustomerID = model.CustomerID,
                Status = "Unfulfilled" // Set the initial status here
            };

            await _orderService.Create(order);

            foreach (var orderDetail in selectedProducts)
            {
                var multipleOrderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity
                };

                await _orderDetailService.Create(multipleOrderDetail);
            }
            
            return RedirectToAction("Index", "Order"); // Redirect to the order index page
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            List<Customer> customers = await _customerService.GetAll();
            SelectList customerList = new SelectList(customers, "CustomerID", "CustomerFullName");
            // Get the product by its ID
            var order = await _orderService.GetById(id);

            if (order == null)
            {
                return NotFound(); // Handle the case where the product is not found
            }

            var orderViewModel = new OrderViewModel
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Customer = order.Customer,
                Customers = customerList
            };

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, OrderViewModel updatedOrderViewModel)
        {
            if (id != updatedOrderViewModel.OrderID)
            {
                return BadRequest(); // Handle the case where the ID in the URL and the order ID don't match
            }

            var updatedOrder = new Order
            {
                OrderID = updatedOrderViewModel.OrderID,
                OrderDate = updatedOrderViewModel.OrderDate,
                CustomerID = updatedOrderViewModel.CustomerID,
                Status = updatedOrderViewModel.Status
            };

            var updatedEntity = await _orderService.Update(id, updatedOrder);

            if (updatedEntity == null)
            {
                return NotFound(); // Handle the case where the update was not successful
            }

            return RedirectToAction("Index"); // Redirect to the product index page after successful update
        }

        public async Task<ActionResult> UpdateStatus(int orderId, string status)
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
