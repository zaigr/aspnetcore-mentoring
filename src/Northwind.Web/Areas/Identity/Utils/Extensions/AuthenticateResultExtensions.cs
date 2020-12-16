using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Northwind.Web.Areas.Identity.Utils.Extensions
{
    public static class AuthenticateResultExtensions
    {
        private const string AuthProviderKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static ExternalLoginInfo GetExternalLoginInfo(this AuthenticateResult result)
        {
            if (result?.Principal == null || result.Properties?.Items == null)
            {
                return null;
            }

            var providerKey = result.Principal.FindFirstValue(AuthProviderKey);
            var provider = result.Properties.Items["LoginProvider"];

            if (providerKey == null || provider == null)
            {
                return null;
            }

            return new ExternalLoginInfo(result.Principal, provider, providerKey, provider)
            {
                AuthenticationTokens = result.Properties.GetTokens(),
                AuthenticationProperties = result.Properties,
            };
        }
    }
}
