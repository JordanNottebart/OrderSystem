using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.UI.Controllers
{
    public class CustomerController : Controller
    {
        CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: CustomerController
        public async Task<ActionResult> Index()
        {
            var customers = await _customerService.GetAll();
            return View(customers);
        }

        // GET: CustomerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var customer = await _customerService.GetById(id);
            return View(customer);
        }

        // GET: CustomerController/Create
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            int lastCustomerId = await _customerService.GetLastId();

            var customer = new Customer
            {
                CustomerID = lastCustomerId + 1
            };

            return View(customer);
        }

        // POST: CustomerController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.Create(customer);
                return RedirectToAction("Index", "Customer"); // Redirect to the customer listing page
            }

            return View(customer);
        }

        // GET: CustomerController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Get the customer by its ID
            var customer = await _customerService.GetById(id);

            if (customer == null)
            {
                return NotFound(); // Handle the case where the customer is not found
            }

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
                var existingCustomer = await _customerService.GetById(id);

                if (existingCustomer == null)
                {
                    return NotFound(); // Handle the case where the product is not found
                }

                existingCustomer.CustomerLastName = updatedCustomer.CustomerLastName;
                existingCustomer.CustomerFirstName = updatedCustomer.CustomerFirstName;
                existingCustomer.Address = updatedCustomer.Address;
                existingCustomer.City = updatedCustomer.City;
                existingCustomer.PostalCode = updatedCustomer.PostalCode;
                existingCustomer.Email = updatedCustomer.Email;
                existingCustomer.Phone = updatedCustomer.Phone;

                var updatedEntity = await _customerService.Update(id, existingCustomer);

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
