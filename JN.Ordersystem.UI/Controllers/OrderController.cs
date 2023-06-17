using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using JN.Ordersystem.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Versioning;

namespace JN.Ordersystem.UI.Controllers
{
    public class OrderController : Controller
    {
        readonly AbstractOrderService _orderService;
        readonly AbstractCustomerService _customerService;
        readonly AbstractProductService _productService;
        readonly AbstractOrderDetailService _orderDetailService;

        public OrderController(AbstractOrderService orderService, AbstractCustomerService customerService, AbstractProductService productService, AbstractOrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// GET: Order
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            // Retrieve all the orders
            var orders = await _orderService.GetAll();

            return View(orders);
        }

        /// <summary>
        /// GET: Order/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int id)
        {
            // Retrieve the order with the specified ID
            var order = await _orderService.GetById(id);

            return View(order);
        }

        /// <summary>
        /// GET: Order/Create
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An OrderViewModel that has a list of OrderDetailViewModels</returns>
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            // Get all the customers
            List<Customer> customers = await _customerService.GetAll();

            // Get all the products
            List<Product> products = await _productService.GetAll();

            // Create a customer and product select list for the dropdown
            SelectList customerList = new SelectList(customers, "CustomerID", "CustomerFullName");
            SelectList productList = new SelectList(products, "ProductID", "ProductFull");

            // Get the current date and time
            DateTime now = DateTime.Now;

            // Get the current date and time with seconds set to 0
            DateTime dateTimeWithSecondsZero = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            // Create a new list with OrderDetailViewModels
            var orderDetailsList = new List<OrderDetailViewModel>();

            // Create a new OrderDetailViewModel and give it the selectlist for the products
            var initOrderDetail = new OrderDetailViewModel
            {
                Products = productList
            };

            // Add the newly created OrderDetailViewModel to the list with OrderDetailViewModels
            orderDetailsList.Add(initOrderDetail);

            // Create a new OrderViewModel with the properties
            var order = new OrderViewModel
            {
                OrderDate = dateTimeWithSecondsZero,
                Customers = customerList,
                OrderDetails = orderDetailsList
            };

            return View(order);
        }

        /// <summary>
        /// POST: Order/Create
        /// </summary>
        /// <param name="model">An OrderViewModel</param>
        /// <param name="selectedProducts">A list of OrderDetailViewModels</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(OrderViewModel model, List<OrderDetailViewModel> selectedProducts)
        {
            // Check if the customerID is 0 or if the list of orderdetails is empty
            if (model.CustomerID == 0 || selectedProducts.Count == 0)
            {
                // Give an error
                ModelState.AddModelError("", "Please select a customer and add at least one product to the cart.");

                // Retrieve the customers and products again to pass to the view
                List<Customer> customers = await _customerService.GetAll();
                List<Product> products = await _productService.GetAll();
                model.Customers = new SelectList(customers, "CustomerID", "CustomerFullName");
                model.OrderDetails[0].Products = new SelectList(products, "ProductID", "ProductFull");
                return View(model);
            }

            // Create the order based on the OrderViewModel that was received
            var order = new Order
            {
                OrderDate = model.OrderDate,
                CustomerID = model.CustomerID,
                Status = "Unfulfilled" // Set the initial status here
            };

            // Create the order and save it to the database
            await _orderService.Create(order);

            // Go through each OrderDetailViewModel in the list
            foreach (var orderDetail in selectedProducts)
            {
                // Create an orderDetail based on the OrderDetailViewModel that was received
                var multipleOrderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity
                };

                // Create the orderDetail and save it to the database
                await _orderDetailService.Create(multipleOrderDetail);
            }

            // Redirect to the order index page
            return RedirectToAction("Index", "Order");
        }

        /// <summary>
        /// GET: Order/Edit/5
        /// </summary>
        /// <param name="id">The OrderID</param>
        /// <returns>An OrderViewModel that has a list of OrderDetailViewModels</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Get all the customers
            List<Customer> customers = await _customerService.GetAll();

            // Get all the products
            List<Product> products = await _productService.GetAll();

            // Create a customer and product select list for the dropdown
            SelectList customerList = new SelectList(customers, "CustomerID", "CustomerFullName");
            SelectList productList = new SelectList(products, "ProductID", "ProductFull");

            // Get the product by its ID
            var order = await _orderService.GetById(id);

            // If the order was not found
            if (order == null)
            {
                // Return a blank page for now
                return NotFound();
            }

            // Create a new list with OrderDetailViewModels
            var orderDetailViewModelList = new List<OrderDetailViewModel>();

            // Go through the list of OrderDetails from the Order that was received
            foreach (var orderDetail in order.OrderDetail)
            {
                // Create for every OrderDetail a new OrderDetailViewModel
                var orderDetailViewModel = new OrderDetailViewModel
                {
                    OrderDetailViewID = orderDetail.OrderDetailID,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity,
                    Products = productList,
                    Product = orderDetail.Product
                };

                // Then add the newly created OrderDetailViewModel to the list
                orderDetailViewModelList.Add(orderDetailViewModel);
            }

            // Create a new OrderViewModel based on the order that was received
            var orderViewModel = new OrderViewModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                CustomerID = order.CustomerID,
                Customer = order.Customer,
                Customers = customerList,
                OrderDetails = orderDetailViewModelList,
                Status = order.Status,
            };

            return View(orderViewModel);
        }

        /// <summary>
        /// POST: Order/Edit/5
        /// </summary>
        /// <param name="id">The id of the OrderViewModel </param>
        /// <param name="updatedOrderViewModel">The OrderViewModel itself</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(int id, OrderViewModel updatedOrderViewModel)
        {
            // Get the original order that the user tried to edit by its ID
            var originalOrder = await _orderService.GetById(id);

            // Get all the orderDetails of the original order
            var originalOrderDetails = originalOrder.OrderDetail;

            // Get the info of the customer of the original order
            var customer = await _customerService.GetById(originalOrder.CustomerID);

            // Go through the list of OrderDetailViewModels from the OrderViewModel that was received
            foreach (var updatedOrderDetailViewModel in updatedOrderViewModel.OrderDetails)
            {
                // Retrieve the info of the product that was selected in the OrderDetailViewModel
                var product = await _productService.GetById(updatedOrderDetailViewModel.ProductID);

                // Get the specific original OrderDetail that the user tried to edit by its ID
                var originalOrderDetail = await _orderDetailService.GetById(updatedOrderDetailViewModel.OrderDetailViewID);

                // Check if the Quantity in the OrderDetailViewModel is greater than the Units in stock for that specific product 
                if (updatedOrderDetailViewModel.Quantity > product.UnitsInStock)
                {
                    // Give an error message
                    ModelState.AddModelError("", "The quantity chosen for product: " + product.ProductFull + " is higher than the Units in Stock!");

                    // Retrieve the customers and products again to pass to the view
                    List<Customer> customers = await _customerService.GetAll();
                    List<Product> products = await _productService.GetAll();
                    SelectList customerList = new SelectList(customers, "CustomerID", "CustomerFullName");
                    SelectList productList = new SelectList(products, "ProductID", "ProductFull");

                    #region Return the original order back to the view
                    // Give the select list of customers back to the OrderViewModel
                    updatedOrderViewModel.Customers = customerList;

                    // Assign the OrderDate of the OrderViewModel, the original OrderDate of the original order
                    updatedOrderViewModel.OrderDate = originalOrder.OrderDate;

                    // Assign the customer of the OrderViewModel, the original customer of the original order
                    updatedOrderViewModel.Customer = customer;

                    // Assign the value of every property of the OrderDetailViewModel to the value of every property of the original OrderDetails
                    for (int index = 0; index < updatedOrderViewModel.OrderDetails.Count; index++)
                    {
                        updatedOrderViewModel.OrderDetails[index].Products = productList;
                        updatedOrderViewModel.OrderDetails[index].Product = originalOrderDetails[index].Product;
                        updatedOrderViewModel.OrderDetails[index].Quantity = originalOrderDetails[index].Quantity;
                    } 
                    #endregion

                    return View(updatedOrderViewModel);
                }
                // If the Quantity in the OrderDetailViewModel is lower than the Units in stock

                // Change the values of the properties of the original OrderDetail to the values of the properties of the updatedOrderDetailViewModel
                originalOrderDetail.ProductID = updatedOrderDetailViewModel.ProductID;
                originalOrderDetail.Quantity = updatedOrderDetailViewModel.Quantity;

                // Update the OrderDetail and save it to the database
                await _orderDetailService.Update(originalOrderDetail.OrderDetailID, originalOrderDetail);
            }

            // Change the value of the properties of the original Order to the value of the updatedOrderViewModel
            originalOrder.OrderDate = updatedOrderViewModel.OrderDate;
            originalOrder.CustomerID = updatedOrderViewModel.CustomerID;

            // Update the Order and save it to the database
            await _orderService.Update(originalOrder.OrderID, originalOrder);

            // Redirect to the order index page after successful update
            return RedirectToAction("Index"); 
        }

        /// <summary>
        /// GET: Order/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int id)
        {
            // Delete the Order and all the OrderDetails associated with it
            await _orderService.Delete(id);

            // Redirect to the order index page
            return RedirectToAction("Index");
        }

        /// <summary>
        /// AJAX Call Post: Order/UpdateStatus
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <param name="status">The status of the order</param>
        /// <returns>A JSON object</returns>
        public async Task<ActionResult> UpdateStatus(int orderId, string status)
        {
            // Retrieve the order with the specified ID
            var order = await _orderService.GetById(orderId);

            // If the order is not found
            if (order == null)
            {
                // Return a blank page for now
                return NotFound();
            }

            // Retrieve all OrderDetails associated with the Order
            var orderDetails = await _orderDetailService.GetAllOrderDetailsByOrderId(orderId);

            // Declare a variable to hold the product to be checked
            Product productToCheck;

            // Go through the list of OrderDetails
            foreach (var orderDetail in orderDetails)
            {
                // Retrieve the info of the product in the orderDetail
                productToCheck = await _productService.GetById(orderDetail.ProductID);

                // Check if the Quantity in the OrderDetail is greater than the Units in stock for that specific product
                if (orderDetail.Quantity > productToCheck.UnitsInStock)
                {
                    // If the Quantity is greater, return a JSON object with details of the failed operation
                    return Json(new 
                    {
                        succes = false, 
                        quantityProduct = orderDetail.Quantity, 
                        unitsInStockProduct = productToCheck.UnitsInStock, 
                        productName = $"{productToCheck.ProductID}. {productToCheck.ItemName}" 
                    });
                }
            }

            // If everything was OK, then change the orderStatus to the status recieved from the Post
            order.Status = status;

            // Update the Order that was changed and save it to the database
            await _orderService.Update(orderId, order);

            // Retrieve the newly updated order
            var updatedOrder = await _orderService.GetById(orderId);

            // To return it as a JSON object
            return Json(new { success = true, data = updatedOrder });
        }

        /// <summary>
        /// AJAX Call Post: Order/UpdateUnitsInStock
        /// </summary>
        /// <param name="orderId">The ID of the Order</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateUnitsInStock(int orderId)
        {
            // Retrieve the order with the specified ID
            var order = await _orderService.GetById(orderId);

            // If the order is not found
            if (order == null)
            {
                // Return a blank page for now
                return NotFound();
            }

            // Go through the list of OrderDetails
            foreach (var orderDetail in order.OrderDetail)
            {
                // Retrieve the info of the product in the orderDetail
                var product = await _productService.GetById(orderDetail.ProductID);

                // Check if the product is found and check if the Quantity in the OrderDetail is greater than the Units in stock for that specific product
                if (product != null && orderDetail.Quantity <= product.UnitsInStock)
                {
                    // Subtract the Quantity from the Units in Stock
                    product.UnitsInStock -= orderDetail.Quantity;

                    // Update the product and save it to the database
                    await _productService.Update(product.ProductID, product);
                }
                else
                {
                    // Else, return a JSON object with details of the failed operation 
                    return Json(new
                    {
                        succes = false,
                        quantityProduct = orderDetail.Quantity,
                        unitsInStockProduct = product.UnitsInStock,
                        productName = $"{product.ProductID}. {product.ItemName}"
                    });
                }
            }

            // Redirect to the order index page
            return RedirectToAction("Index", "Order");
        }
    }
}
