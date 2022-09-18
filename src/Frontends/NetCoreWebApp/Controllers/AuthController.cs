using Microsoft.AspNetCore.Mvc;
using NetCoreWebApp.Dtos;

namespace NetCoreWebApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
