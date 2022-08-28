using Catalog.API.Dtos;
using Catalog.API.Services;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet] public async Task<IActionResult> GetAllAsync() => CreateActionResultInstance(await _categoryService.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id) => CreateActionResultInstance(await _categoryService.GetByIdAsync(id));
        [HttpPost] public async Task<IActionResult> CreateAsync(CategoryCreateDto categoryCreateDto) => CreateActionResultInstance(await _categoryService.CreateAsync(categoryCreateDto));
    }
}