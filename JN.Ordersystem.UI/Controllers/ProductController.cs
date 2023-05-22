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
        public ActionResult Index()
        {
            return View(_productService.GetAll());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View(_productService.GetById(id));
        }

        // GET: ProductController/Create
        public ActionResult Create(Product product)
        {
            return View(_productService.Create(product));
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            _productService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
