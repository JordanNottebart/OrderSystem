using JN.Ordersystem.API.Dto;
using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        IService<OrderDetail> _orderDetailService;

        public OrderDetailController(IService<OrderDetail> orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// Get all the orderDetails
        /// </summary>
        /// <returns>A list of orderDetails</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listOfOrderDetails = await _orderDetailService.GetAll();

                return Ok(listOfOrderDetails);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong please try again" });
            }
        }

        /// <summary>
        /// Get an orderDetail based on the chosen ordersDetailID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific orderDetail</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Search the specific orderDetail
                var orderDetail = await _orderDetailService.GetById(id);

                if (orderDetail == null)
                {
                    return NotFound("Order detail not found");
                }

                return Ok(orderDetail);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }

        }

        /// <summary>
        /// Create a new orderDetail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>A newly created orderDetail</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OrderDetailDto orderDetail)
        {
            try
            {
                // Add a new orderDetail to the list
                var createdOrderDetail = await _orderDetailService.Create(new OrderDetail
                {
                    OrderID = orderDetail.OrderID,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity
                });

                // Return the statuscode 201 (created)
                return CreatedAtAction("GetById", new { id = createdOrderDetail.OrderDetailID }, createdOrderDetail);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Update the whole orderDetail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderDetail"></param>
        /// <returns>An updated orderDetail</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDetailDto orderDetail)
        {
            try
            {
                // Update the orderDetail
                var orderDetailToUpdate = await _orderDetailService.Update(id, new OrderDetail
                {
                    OrderID = orderDetail.OrderID,
                    ProductID = orderDetail.ProductID,
                    Quantity = orderDetail.Quantity
                });

                // Return 404 if the orderDetail is not found
                if (orderDetailToUpdate == null)
                {
                    return NotFound("Order detail not found");
                }

                // Returns a CreatedAtAction result with the newly created orderDetail
                return CreatedAtAction("GetById", new { id = orderDetailToUpdate.OrderDetailID }, orderDetailToUpdate);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Deletes an orderDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a response based on if the action was successful or not</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Delete the orderDetail based on its id
                var isDeleted = await _orderDetailService.Delete(id);

                if (isDeleted)
                {
                    // Return an OK response
                    return Ok(new { Message = "Order detail has been deleted successfully" });
                }

                // Return a BadRequest response if the orderDetail was not deleted
                return BadRequest(new { Message = "Something went wrong trying to delete the order detail." });
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }
    }
}
