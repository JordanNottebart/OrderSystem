using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.UI.Controllers
{
    public class CustomerController : Controller
    {
        readonly AbstractCustomerService _customerService;

        public CustomerController(AbstractCustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// GET: Customer
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            // Retrieve all customers from the customer service
            var customers = await _customerService.GetAll();

            return View(customers);
        }

        /// <summary>
        /// GET: Customer/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int id)
        {
            // Retrieve the customer with the specified ID from the customer service
            var customer = await _customerService.GetById(id);

            return View(customer);
        }

        /// <summary>
        /// GET: Customer/Create
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            // Create a new instance of Customer
            var customer = new Customer();

            return View(customer);
        }

        /// <summary>
        /// POST: Customer/Create
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Create a new customer
                await _customerService.Create(customer);
                // Redirect to the customer index page
                return RedirectToAction("Index", "Customer"); 
            }

            return View(customer);
        }

        /// <summary>
        /// GET: Customer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Get the customer by its ID
            var customer = await _customerService.GetById(id);

            // If the customer is not found by its id
            if (customer == null)
            {
                // Return a blank page for now
                return NotFound();
            }

            // Pass the customer that was found to the view
            return View(customer);
        }

        /// <summary>
        /// POST: Customer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedCustomer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Customer updatedCustomer)
        {
            if (ModelState.IsValid)
            {
                // Update the customer
                var customerToUpdate = await _customerService.Update(id, updatedCustomer);

                // If the customer is not found by its id
                if (customerToUpdate == null)
                {
                    // Return a blank page for now
                    return NotFound(); 
                }

                // Redirect to the customer index page after successful update
                return RedirectToAction("Index"); 
            }

            // If the model that the user tried to post was not valid, pass the beginning customer back to the view
            return View(updatedCustomer);
        }

        /// <summary>
        /// GET: Customer/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int id)
        {
            // Delete the customer
            var isDeleted = await _customerService.Delete(id);

            // If the delete was succesful
            if (isDeleted)
            {
                return RedirectToAction("Index", "Customer");
            }

            // Else return the details of the customer for now
            return View("Details", id);
        }
    }
}
