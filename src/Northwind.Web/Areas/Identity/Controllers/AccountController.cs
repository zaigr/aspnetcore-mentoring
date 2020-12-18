using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Northwind.Web.Areas.Identity.Utils.Extensions;

namespace Northwind.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[area]/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IUserStore<IdentityUser> _userStore;

        private readonly IUserEmailStore<IdentityUser> _emailStore;

        private readonly IEmailSender _emailSender;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;

            _emailStore = GetEmailStore();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var authenticationResult = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);

            var externalInfo = authenticationResult.GetExternalLoginInfo();
            if (externalInfo == null)
            {
                var errorMessage = "Error login external login information during configuration.";
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = returnUrl, ErrorMessage = errorMessage });
            }

            var externalUser = await _userManager.FindByLoginAsync(externalInfo.LoginProvider, externalInfo.ProviderKey);
            if (externalUser == null)
            {
                var user = new IdentityUser();
                var createResult = await InitUserAsync(user, externalInfo);
                if (!createResult.Succeeded)
                {
                    var errorMessage = createResult.Errors.FirstOrDefault();
                    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = returnUrl, errorMessage = errorMessage });
                }

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddLoginAsync(user, externalInfo);

                    await SendConfirmationEmailAsync(user);

                    await _signInManager.SignOutAsync();

                    return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity", Email = user.Email });
                }
            }

            await UpdateUserRoleAsync(externalUser, externalInfo);

            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            await _signInManager.SignInAsync(externalUser, isPersistent: true);

            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        private async Task UpdateUserRoleAsync(IdentityUser externalUser, ExternalLoginInfo externalInfo)
        {
            var roles = await _userManager.GetRolesAsync(externalUser);
            if (roles.Count > 1)
            {
                throw new InvalidOperationException("Application support only single role assigned per user.");
            }

            var currentRole = roles.SingleOrDefault();
            var externalRole = externalInfo.GetUserRole();

            var isRoleChanged = currentRole != externalRole;
            if (isRoleChanged && currentRole != null)
            {
                await _userManager.RemoveFromRoleAsync(externalUser, currentRole);
            }

            if (isRoleChanged && externalRole != null)
            {
                await _userManager.AddToRoleAsync(externalUser, externalRole);
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user sore with email support.");
            }

            return (IUserEmailStore<IdentityUser>)_userStore;
        }

        private async Task<IdentityResult> InitUserAsync(IdentityUser user, ExternalLoginInfo externalInfo)
        {
            var email = externalInfo.Principal.Identity.Name;
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user);

            var role = externalInfo.GetUserRole();
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return result;
        }

        private async Task SendConfirmationEmailAsync(IdentityUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
