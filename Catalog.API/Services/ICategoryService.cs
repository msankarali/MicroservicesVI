using Catalog.API.Dtos;
using Catalog.API.Models;
using Utilities.Results;

namespace Catalog.API.Services
{
    internal interface ICategoryService
    {
        Task<ApiResponse<List<CategoryDto>>> GetAllAsync();
        Task<ApiResponse<CategoryDto>> CreateAsync(Category category);
        Task<ApiResponse<CategoryDto>> GetByIdAsync(string id);
    }
}