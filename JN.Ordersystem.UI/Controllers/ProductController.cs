using JN.Ordersystem.BL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.UI.Controllers
{
    public class ProductController : Controller
    {
        ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            var products = await _productService.GetAll();
            return View(products);
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var product = await _productService.GetById(id);
            return View(product);
        }

        // GET: /Product/Create
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            int lastProductId = await _productService.GetLastId();

            var product = new Product
            {
                ProductID = lastProductId + 1
            };

            return View(product);
        }

        // POST: /Product/Create
        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.Create(product);
                return RedirectToAction("Index", "Product"); // Redirect to the product listing page
            }

            return View(product);
        }

        // GET: ProductController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Get the product by its ID
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound(); // Handle the case where the product is not found
            }

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Product updatedProduct)
        {
            if (id != updatedProduct.ProductID)
            {
                return BadRequest(); // Handle the case where the ID in the URL and the product ID don't match
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _productService.GetById(id);

                if (existingProduct == null)
                {
                    return NotFound(); // Handle the case where the product is not found
                }

                existingProduct.Description = updatedProduct.Description;
                existingProduct.ItemName = updatedProduct.ItemName;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.UnitsInStock = updatedProduct.UnitsInStock;

                var updatedEntity = await _productService.Update(id, existingProduct);

                if (updatedEntity == null)
                {
                    return NotFound(); // Handle the case where the update was not successful
                }

                return RedirectToAction("Index"); // Redirect to the product index page after successful update
            }

            return View(updatedProduct); // Return the view with validation errors if the model is not valid
        }


        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
