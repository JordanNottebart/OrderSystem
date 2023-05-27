using JN.Ordersystem.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JN.Ordersystem.UI.Controllers
{
    public class OrderController : Controller
    {
        OrderService _orderService;

        public OrderController(OrderService service)
        {
            _orderService = service;
        }

        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            return View(await _orderService.GetAll());
        }

    }
}
