using JN.Ordersystem.API.Dto;
using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        AbstractOrderService _orderService;

        public OrderController(AbstractOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get all the orders
        /// </summary>
        /// <returns>A list of orders</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listOfOrders = await _orderService.GetAll();

                return Ok(listOfOrders);
            }
            catch (Exception e)
            {

                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong please try again" });
            }
        }

        /// <summary>
        /// Get an order based on the chosen ordersID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific order</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Search the specific order
                var order = await _orderService.GetById(id);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                return Ok(order);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }

        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>A newly created order</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OrderDto order)
        {
            try
            {
                // Add a new order to the list
                var createdOrder = await _orderService.Create(new Order
                {
                    OrderDate = order.OrderDate,
                    CustomerID = order.CustomerID,
                    Status = order.Status
                });

                // Return the statuscode 201 (created)
                return CreatedAtAction("GetById", new { id = createdOrder.OrderID }, createdOrder);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Update the whole order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns>An updated order</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDto order)
        {
            try
            {
                // Update the order
                var orderToUpdate = await _orderService.Update(id, new Order
                {
                    OrderDate = order.OrderDate,
                    CustomerID = order.CustomerID,
                    Status = order.Status
                });

                // Return 404 if the order is not found
                if (orderToUpdate == null)
                {
                    return NotFound("Order not found");
                }

                // Returns a CreatedAtAction result with the newly created order
                return CreatedAtAction("GetById", new { id = orderToUpdate.OrderID }, orderToUpdate);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a response based on if the action was successful or not</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Delete the order based on its id
                var isDeleted = await _orderService.Delete(id);

                if (isDeleted)
                {
                    // Return an OK response
                    return Ok(new { Message = "Order has been deleted successfully" });
                }

                // Return a BadRequest response if the order was not deleted
                return BadRequest(new { Message = "Something went wrong trying to delete the order." });
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }
    }
}
