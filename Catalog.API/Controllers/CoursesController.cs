using Catalog.API.Dtos;
using Catalog.API.Services;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    internal class CoursesController : BaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var response = await _courseService.GetByIdAsync(id);

            return CreateActionResultInstance(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllByUserIdAsync(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);

            return CreateActionResultInstance(response);
        }

        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _courseService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var response = await _courseService.CreateAsync(courseCreateDto);

            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDto);

            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _courseService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }
    }
}