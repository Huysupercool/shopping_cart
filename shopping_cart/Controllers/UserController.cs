using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using shopping_cart.Models;
using System.Text.Encodings.Web;
using System.Text;
using shopping_cart.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace shopping_cart.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserStore<AppUser> _userStore;
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, IUserStore<AppUser> userStore)
        {
            _dbContext = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userModel)
        {
            string returnUrl = Url.Content("~/");
            // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.Email = userModel.Email;
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                await _userStore.SetUserNameAsync(user, userModel.Email, CancellationToken.None);
                //  await _emailStore.SetEmailAsync(user, userModel.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded)
                {
                    //  _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = "" },
                        protocol: Request.Scheme);

                    //  await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //      $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    ViewBag.EmailConfirmationUrl = callbackUrl;
                    ViewBag.DisplayConfirmAccountLink = true;
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return View("RegisterConfirmation", new { email = userModel.Email, returnUrl = "" });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("Home/Index");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(loginViewModel userModel)
        {
            string returnUrl = Url.Content("~/Home/Index");

            // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning("User account locked out.");
                //    return RedirectToPage("./Lockout");
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return Page();
                //}
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        public async Task<IActionResult> Logout(string returnUrl = null) {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return LocalRedirect("Home/Index");
        }

    private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        //private IUserEmailStore<IdentityUser> GetEmailStore()
        //{
        //    if (!_userManager.SupportsUserEmail)
        //    {
        //        throw new NotSupportedException("The default UI requires a user store with email support.");
        //    }
        //    return (IUserEmailStore<IdentityUser>)_userStore;
        //}
    }
}
