using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.UI.Controllers
{
    public class CustomerController : Controller
    {
        readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: CustomerController
        public async Task<ActionResult> Index()
        {
            // Retrieve all customers from the customer service
            var customers = await _customerService.GetAll();

            return View(customers);
        }

        // GET: CustomerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // Retrieve the customer with the specified ID from the customer service
            var customer = await _customerService.GetById(id);

            return View(customer);
        }

        // GET: CustomerController/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            // Create a new instance of Customer
            var customer = new Customer();

            return View(customer);
        }

        // POST: CustomerController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.Create(customer);
                return RedirectToAction("Index", "Customer"); // Redirect to the customer index page
            }

            return View(customer);
        }

        // GET: CustomerController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Get the customer by its ID
            var customer = await _customerService.GetById(id);

            // If the customer is not found by its id
            if (customer == null)
            {
                // Return a blank page
                return NotFound();
            }

            // Pass the customer that was found to the view
            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Customer updatedCustomer)
        {
            if (id != updatedCustomer.CustomerID)
            {
                return BadRequest(); // Handle the case where the ID in the URL and the product ID don't match
            }

            if (ModelState.IsValid)
            {
                var updatedEntity = await _customerService.Update(id, updatedCustomer);

                if (updatedEntity == null)
                {
                    return NotFound(); // Handle the case where the update was not successful
                }

                return RedirectToAction("Index"); // Redirect to the product index page after successful update
            }

            return View(updatedCustomer);
        }

        // GET: CustomerController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await _customerService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
