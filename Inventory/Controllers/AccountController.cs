using Inventory.Models.Account;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Inventory.ViewModels.AccountVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace Inventory.Controllers
{
    [Route("Usuario")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
            return View(await _accountService.GetUsersByPaggingList(filter, pageindex, sort));
        }

        [Route("Login/")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [Route("Login/")]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user,
                    loginVM.Password, true, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Warehouses");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }
            TempData["user"] = "Usuário e/ou senha inválidos!";
            return View(loginVM);
        }

        [Route("Cadastro/")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
            List<Role> roles = await _accountService.GetAllRoles();

            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        [Route("Cadastro/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.CreateUser(userVM);

                if (user != null)
                {
                    TempData["successMessage"] = "Usuário " + userVM.UserName;
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    this.ModelState.AddModelError("Registro", "Falha ao registrar o usuário");
                }
            }
            TempData["errorMessage"] = "Usuário " + userVM.UserName;
            return View(userVM);
        }

        [Route("Mudar-Senha/")]
        public IActionResult ChangePassword(int userId)
        {
            ChangePasswordViewModel userReturn = new ChangePasswordViewModel();

            userReturn.UserId = userId;
            return View(userReturn);
        }

        [HttpPost]
        [Route("Mudar-Senha/")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel userVM, int userId)
        {
            if (!ModelState.IsValid)
                return View(userVM);
            try
            {
                var user = await _accountService.GetUserByIdAsync(userVM.UserId);

                if (user != null && user.Id == userId)
                {
                    bool result = _accountService.UpdateUser(user, userVM);

                    if (result)
                    {
                        return RedirectToAction("Index", "Warehouses");
                    }
                }

                TempData["user"] = "Erro ao mudar senha!";
                return View(userVM);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Route("AcessoNegado")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
