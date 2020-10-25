using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Web.ViewModels.Shared;

namespace Northwind.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
