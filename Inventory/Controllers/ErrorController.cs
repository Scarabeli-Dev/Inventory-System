using Inventory.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory.Controllers
{
    [Route("Erro")]
    public class ErrorController : Controller
    {

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
