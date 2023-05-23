using JN.Ordersystem.API.Dto;
using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IService<Product> _productService;

        public ProductController(IService<Product> productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all the products
        /// </summary>
        /// <returns>A list of products</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listOfProducts = await _productService.GetAll();

                return Ok(listOfProducts);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Something went wrong please try again" });
            }
        }

        /// <summary>
        /// Get a product based on the chosen productID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific product</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Search the specific product
                var product =  await _productService.GetById(id);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(product);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
            
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>A newly created product</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProductDto product)
        {
            try
            {
                // Add a new product to the list
                var createdProduct = await _productService.Create(new Product
                {
                    ItemName= product.ItemName,
                    Description = product.Description,
                    Price = product.Price,
                    UnitsInStock = product.UnitsInStock
                });

                // Return the statuscode 201 (created)
                return CreatedAtAction("GetById", new { id = createdProduct.ProductID }, createdProduct);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Update the whole product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>An updated product</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto product)
        {
            try
            {
                // Update the product
                var productToUpdate = await _productService.Update(id, new Product
                {
                    ItemName = product.ItemName,
                    Description = product.Description,
                    Price = product.Price,
                    UnitsInStock = product.UnitsInStock
                });

                // Return 404 if the product is not found
                if (productToUpdate == null)
                {
                    return NotFound("Product not found");
                }

                // Returns a CreatedAtAction result with the newly created product
                return CreatedAtAction("GetById", new { id = productToUpdate.ProductID }, productToUpdate);
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a response based on if the action was successful or not</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Delete the product based on its id
                var isDeleted = await _productService.Delete(id);

                if (isDeleted)
                {
                    // Return an OK response
                    return Ok(new { Message = "Product has been deleted successfully" });
                }

                // Return a BadRequest response if the product was not deleted
                return BadRequest(new { Message = "Something went wrong trying to delete the product." });
            }
            catch (Exception e)
            {
                // Return an error code if something went wrong
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }
    }
}
