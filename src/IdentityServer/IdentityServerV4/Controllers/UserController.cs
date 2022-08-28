using IdentityServerV4.Dtos;
using IdentityServerV4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Results;

namespace IdentityServerV4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var user = new ApplicationUser
            {
                UserName = signUpDto.UserName,
                City = signUpDto.City,
                Email = signUpDto.Email
            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(ApiResponse<NoContent>.Fail(400, result.Errors
                    .Select(e => e.Description)
                    .Aggregate((a, b) =>
                    {
                        return a + ", " + b;
                    })));
            }

            return NoContent();
        }
    }
}