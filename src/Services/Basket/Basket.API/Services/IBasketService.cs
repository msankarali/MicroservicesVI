using Basket.API.Dtos;
using Utilities.Results;

namespace Basket.API.Services
{
    public interface IBasketService
    {
        Task<ApiResponse<BasketDto>> GetAsync(string userId);
        Task<ApiResponse<bool>> SaveOrUpdateAsync(BasketDto basketDto);
        Task<ApiResponse<bool>> DeleteAsync(string userId);
    }
}