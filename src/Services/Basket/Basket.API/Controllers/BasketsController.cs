using Basket.API.Dtos;
using Basket.API.Services;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Identity;
using Utilities.Results;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : BaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet] public async Task<IActionResult> GetAsync() => CreateActionResultInstance(await _basketService.GetAsync(_sharedIdentityService.GetUserId));
        [HttpPost] public async Task<IActionResult> PostAsync(BasketDto basketDto) => CreateActionResultInstance(await _basketService.SaveOrUpdateAsync(basketDto));
        [HttpDelete] public async Task<IActionResult> DeleteAsync() => CreateActionResultInstance(await _basketService.DeleteAsync(_sharedIdentityService.GetUserId));
    }
}