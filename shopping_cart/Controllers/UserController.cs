using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using shopping_cart.Models;
using System.Text.Encodings.Web;
using System.Text;
using shopping_cart.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace shopping_cart.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly ILogger<AppUser> _logger;
        private readonly IEmailSender _sender;
        public UserController(IEmailSender sender, ILogger<AppUser> logger, IUserStore<AppUser> userStore, UserManager<AppUser> userManager, AppDbContext context, SignInManager<AppUser> signInManager)
        {
            _sender = sender;
            _logger = logger;
            _signInManager = signInManager;
            _dbContext = context;
            _userManager = userManager;
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
          if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.Email = userModel.Email;
                user.Phone = userModel.Phone;
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Address = userModel.Address;
                user.Age = userModel.Age;
                await _userStore.SetUserNameAsync(user, userModel.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = "" },
                        protocol: Request.Scheme);
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

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(loginViewModel userModel)
        {
            string returnUrl = Url.Content("~/Home/Index");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }
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
    }
}
