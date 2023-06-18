using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.UI.Controllers
{
    public class ProductController : Controller
    {
        readonly AbstractProductService _productService;

        public ProductController(AbstractProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// GET: Product
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            // Retrieve all products
            var products = await _productService.GetAll();

            return View(products);
        }

        /// <summary>
        /// GET: Product/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int id)
        {
            // Retrieve the product with the specified ID
            var product = await _productService.GetById(id);

            return View(product);
        }

        /// <summary>
        /// GET: Product/Create
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            // Create a new instance of Product
            var product = new Product();

            return View(product);
        }

        /// <summary>
        /// POST: Product/Create
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Create a new product
                await _productService.Create(product);

                // Redirect to the product index page
                return RedirectToAction("Index", "Product"); 
            }

            return View(product);
        }

        /// <summary>
        /// GET: Product/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Get the product by its ID
            var product = await _productService.GetById(id);

            // If the product is not found by its id
            if (product == null)
            {
                // Return a blank page for now
                return NotFound(); 
            }

            // Pass the product that was found to the view
            return View(product);
        }

        /// <summary>
        /// POST: Product/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Product updatedProduct)
        {
            if (ModelState.IsValid)
            {
                // Update the product
                var productToUpdate = await _productService.Update(id, updatedProduct);

                // If the product is not found by its id
                if (productToUpdate == null)
                {
                    // Return a blank page for now
                    return NotFound(); 
                }

                // Redirect to the product index page after successful update
                return RedirectToAction("Index"); 
            }

            // If the model that the user tried to post was not valid, pass the beginning product back to the view
            return View(updatedProduct); 
        }

        /// <summary>
        /// GET: Product/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int id)
        {
            // Delete the product
            var isDeleted = await _productService.Delete(id);

            // If the delete was succesful
            if (isDeleted)
            {
                return RedirectToAction("Index", "Product");
            }

            // Else return the details of the product for now
            return View("Details", id);
        }
    }
}
