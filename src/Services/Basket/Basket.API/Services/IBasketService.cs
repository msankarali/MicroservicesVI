using Basket.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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