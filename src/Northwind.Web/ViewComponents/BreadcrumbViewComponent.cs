using Microsoft.AspNetCore.Mvc;

namespace Northwind.Web.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
