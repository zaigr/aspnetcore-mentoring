using Microsoft.AspNetCore.Mvc;

namespace Northwind.Api.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
