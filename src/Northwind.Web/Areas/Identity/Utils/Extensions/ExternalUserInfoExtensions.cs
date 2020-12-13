using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Web.Areas.Identity.Utils.Extensions
{
    public static class ExternalLoginInfoExtensions
    {
        private const string RoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public static string GetUserRole(this ExternalLoginInfo info)
        {
            return info.Principal.Claims.FirstOrDefault(c => c.Type == RoleClaim)?.Value;
        }
    }
}
