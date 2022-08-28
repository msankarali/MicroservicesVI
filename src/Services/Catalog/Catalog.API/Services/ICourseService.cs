using Catalog.API.Dtos;
using Utilities.Results;

namespace Catalog.API.Services
{
    public interface ICourseService
    {
        Task<ApiResponse<List<CourseDto>>> GetAllAsync();
        Task<ApiResponse<CourseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<CourseDto>>> GetAllByUserIdAsync(string userId);
        Task<ApiResponse<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto);
        Task<ApiResponse<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto);
        Task<ApiResponse<NoContent>> DeleteAsync(string id);
    }
}
