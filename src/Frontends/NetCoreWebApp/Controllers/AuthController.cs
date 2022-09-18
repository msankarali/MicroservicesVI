using Microsoft.AspNetCore.Mvc;
using NetCoreWebApp.Dtos;
using NetCoreWebApp.Services.Abstract;

namespace NetCoreWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }



        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignInAsync(SignInDto signInDto)
        {
            if (!ModelState.IsValid)
            {
                return View(signInDto);
            }

            var response = await _identityService.SignIn(signInDto);

            if (!response.IsSuccessful)
            {
                response.Messages.ForEach(m =>
                {
                    ModelState.AddModelError(string.Empty, m);
                });
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
