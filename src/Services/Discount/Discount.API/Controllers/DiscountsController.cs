using Common;
using Discount.API.Services;
using Microsoft.AspNetCore.Mvc;
using Utilities.Identity;

namespace Discount.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountsController : BaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;
        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet] public async Task<IActionResult> GetAllAsync() =>
            CreateActionResultInstance(await _discountService.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(int id) =>
            CreateActionResultInstance(await _discountService.GetByIdAsync(id));
        [HttpGet("{code}")] public async Task<IActionResult> GetByCodeAsync(string code) =>
            CreateActionResultInstance(await _discountService.GetByCodeAndUserIdAsync(code, _sharedIdentityService.GetUserId));
        [HttpPost] public async Task<IActionResult> AddAsync(Entities.Discount discount) =>
            CreateActionResultInstance(await _discountService.AddAsync(discount));
        [HttpPut] public async Task<IActionResult> UpdateAsync(Entities.Discount discount) =>
            CreateActionResultInstance(await _discountService.UpdateAsync(discount));
        [HttpDelete("{id}")] public async Task<IActionResult> DeleteAsync(int id) =>
            CreateActionResultInstance(await _discountService.DeleteByIdAsync(id));
    }
}