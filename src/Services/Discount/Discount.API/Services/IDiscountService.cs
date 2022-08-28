using Utilities.Results;

namespace Discount.API.Services
{
    public interface IDiscountService
    {
        Task<ApiResponse<List<Entities.Discount>>> GetAll();
        Task<ApiResponse<Entities.Discount>> GetById(int id);
        Task<ApiResponse<NoContent>> Add(Entities.Discount discount);
        Task<ApiResponse<NoContent>> Update(Entities.Discount discount);
        Task<ApiResponse<NoContent>> DeleteById(int id);
        Task<ApiResponse<Entities.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}