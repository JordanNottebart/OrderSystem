using JN.Ordersystem.API.Dto;
using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JN.Ordersystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        IService<Customer> _customerService;

        public CustomerController(IService<Customer> customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get all the customers
        /// </summary>
        /// <returns>A list of customers</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listOfCustomers = await _customerService.GetAll();

                return Ok(listOfCustomers);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong please try again" });
            }
        }

        /// <summary>
        /// Get a customer's info based on the chosen customersID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific customer's info</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Find the specific customer
                var customer = await _customerService.GetById(id);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                return Ok(customer);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }

        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>A newly created customer</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CustomerDto customer)
        {
            try
            {
                // Add a new customer to the list
                var createdCustomer = await _customerService.Create(new Customer
                {
                    CustomerLastName = customer.CustomerLastName,
                    CustomerFirstName = customer.CustomerFirstName,
                    Address = customer.Address,
                    City = customer.City,
                    PostalCode = customer.PostalCode,
                    Email = customer.Email,
                    Phone = customer.Phone,
                });

                // Return the statuscode 201 (created)
                return CreatedAtAction("GetById", new { id = createdCustomer.CustomerID }, createdCustomer);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Update all the customer's info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns>An updated customer</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto customer)
        {
            try
            {
                // Update the customer's info
                var customerToUpdate = await _customerService.Update(id, new Customer
                {
                    CustomerLastName = customer.CustomerLastName,
                    CustomerFirstName = customer.CustomerFirstName,
                    Address = customer.Address,
                    City = customer.City,
                    PostalCode = customer.PostalCode,
                    Email = customer.Email,
                    Phone = customer.Phone,
                });

                // Return 404 if the customer is not found
                if (customerToUpdate == null)
                {
                    return NotFound("Customer not found");
                }

                // Returns a CreatedAtAction result with the newly created customer
                return CreatedAtAction("GetById", new { id = customerToUpdate.CustomerID }, customerToUpdate);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        //[HttpPatch("address/{id}")]
        //public IActionResult UpdateCustomerAddress(int id, [FromBody] CustomerDto customer)
        //{
        //    try
        //    {
        //        var customerToPatch = _customerService.patch(id, new Customer
        //        {
        //            Address = customer.Address,
        //            City = customer.City,
        //            PostalCode = customer.PostalCode,
        //        });
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a response based on if the action was successful or not</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Delete the customer based on his id
                var isDeleted = await _customerService.Delete(id);

                if (isDeleted)
                {
                    // Return an OK response
                    return Ok(new { Message = "Customer has been deleted successfully" });
                }

                // Return a BadRequest response if the customer was not deleted
                return BadRequest(new { Message = "Something went wrong trying to delete the customer." });
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }
    }
}
