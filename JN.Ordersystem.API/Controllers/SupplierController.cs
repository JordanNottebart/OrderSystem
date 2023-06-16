using JN.Ordersystem.API.Dto;
using JN.Ordersystem.API.Models;
using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        AbstractSupplierService _supplierService;

        public SupplierController(AbstractSupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Get all the supplier infos
        /// </summary>
        /// <returns>A list of supplier infos</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listOfSuppliers = await _supplierService.GetAll();

                return Ok(listOfSuppliers);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong please try again" });
            }
        }

        /// <summary>
        /// Get a supplier's info based on the chosen supplierID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific supplier's info</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Search the specific supplier
                var supplier = await _supplierService.GetById(id);

                if (supplier == null)
                {
                    return NotFound("Supplier not found");
                }

                return Ok(supplier);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }

        }

        /// <summary>
        /// Create a new supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>A newly created supplier</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SupplierDto supplier)
        {
            try
            {
                // Add a new supplier to the list
                var createdSupplier = await _supplierService.Create(new Supplier
                {
                    SupplierName = supplier.SupplierName,
                    ContactInfo = supplier.ContactInfo,
                });

                // Return the statuscode 201 (created)
                return CreatedAtAction("GetById", new { id = createdSupplier.SupplierID }, createdSupplier);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Update all the supplier's info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="supplier"></param>
        /// <returns>An updated supplier</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierDto supplier)
        {
            try
            {
                // Update the supplier's info
                var supplierToUpdate = await _supplierService.Update(id, new Supplier
                {
                    SupplierName = supplier.SupplierName,
                    ContactInfo = supplier.ContactInfo,
                });

                // Return 404 if the supplier is not found
                if (supplierToUpdate == null)
                {
                    return NotFound("Supplier not found");
                }

                // Returns a CreatedAtAction result with the newly updated supplier
                return CreatedAtAction("GetById", new { id = supplierToUpdate.SupplierID }, supplierToUpdate);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Deletes a supplier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a response based on if the action was successful or not</returns>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Delete the supplier based on his id
                var isDeleted = await _supplierService.Delete(id);

                if (isDeleted)
                {
                    // Return an OK response
                    return Ok(new { Message = "Supplier has been deleted successfully" });
                }

                // Return a BadRequest response if the supplier was not deleted
                return BadRequest(new { Message = "Something went wrong trying to delete the supplier." });
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }
    }
}
