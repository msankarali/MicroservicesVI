using Basket.API.Dtos;
using System.Text.Json;
using Utilities.Results;

namespace Basket.API.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);

            return status
                ? ApiResponse<bool>.Success(204)
                : ApiResponse<bool>.Fail(404, "Basket not found");
        }

        public async Task<ApiResponse<BasketDto>> GetAsync(string userId)
        {
            var basket = await _redisService.GetDb().StringGetAsync(userId);

            if (string.IsNullOrEmpty(basket))
            {
                return ApiResponse<BasketDto>.Fail(404, "Basket not found");
            }

            return ApiResponse<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(basket), 200);
        }

        public async Task<ApiResponse<bool>> SaveOrUpdateAsync(BasketDto basketDto)
        {
            var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            return status
                ? ApiResponse<bool>.Success(204)
                : ApiResponse<bool>.Fail(500, "Basket could not be updated");
        }
    }
}