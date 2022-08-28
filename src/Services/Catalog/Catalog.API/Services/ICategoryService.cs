using Catalog.API.Dtos;
using Catalog.API.Models;
using Utilities.Results;

namespace Catalog.API.Services
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<CategoryDto>>> GetAllAsync();
        Task<ApiResponse<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto);
        Task<ApiResponse<CategoryDto>> GetByIdAsync(string id);
    }
}