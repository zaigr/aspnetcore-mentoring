using Microsoft.AspNetCore.Mvc;

namespace Northwind.Api.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
