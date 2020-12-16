using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Web.Areas.Identity.Data;
using Northwind.Web.Const;

namespace Northwind.Web.Controllers
{
    [Authorize(Roles = UserRoles.Administrator)]
    public class AdministrationController : Controller
    {
        private readonly IdentityContext _identityContext;

        public AdministrationController(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _identityContext.Users.ToListAsync();

            return View(users);
        }
    }
}
