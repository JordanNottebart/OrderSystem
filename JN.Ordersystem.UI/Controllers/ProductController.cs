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

        // GET: ProductController/Create
        public async Task<ActionResult> Create(Product product)
        {
            return View(await _productService.Create(product));
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View();
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
