using Utilities.Results;

namespace Discount.API.Services
{
    public interface IDiscountService
    {
        Task<ApiResponse<List<Entities.Discount>>> GetAllAsync();
        Task<ApiResponse<Entities.Discount>> GetByIdAsync(int id);
        Task<ApiResponse<NoContent>> AddAsync(Entities.Discount discount);
        Task<ApiResponse<NoContent>> UpdateAsync(Entities.Discount discount);
        Task<ApiResponse<NoContent>> DeleteByIdAsync(int id);
        Task<ApiResponse<Entities.Discount>> GetByCodeAndUserIdAsync(string code, string userId);
    }
}