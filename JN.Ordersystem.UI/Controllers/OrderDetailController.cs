using JN.Ordersystem.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.UI.Controllers
{
    public class OrderDetailController : Controller
    {
        ProductService _productService;

        public OrderDetailController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: OrderDetailController
        public async Task<ActionResult> Index()
        {
            var products = await _productService.GetAll();
            return View(products);
        }

    }
}
