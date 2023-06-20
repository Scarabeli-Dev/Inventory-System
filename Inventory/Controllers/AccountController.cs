using Inventory.Models.Account;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.AccountVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user,
                    loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Warehouses");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Falha ao realizar o login!!");
            return View(loginVM);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList(string filter, int pageindex = 1, string sort = "Name")
        {
            var model = await _accountService.GetAllUsersByPaggingList(filter, pageindex, sort);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
            List<Role> roles = await _accountService.GetAllRoles();

            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = _accountService.CreateUser(userVM);

                if (user != null)
                {
                    return RedirectToAction("Warehouses", "Index");
                }
                else
                {
                    this.ModelState.AddModelError("Registro", "Falha ao registrar o usuário");
                }
            }
            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
