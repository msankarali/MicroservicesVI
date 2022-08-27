using Catalog.API.Dtos;
using Catalog.API.Services;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    internal class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        internal CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet] public async Task<IActionResult> GetAllAsync() => CreateActionResultInstance(await _categoryService.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> GetByIdAsync(string id) => CreateActionResultInstance(await _categoryService.GetByIdAsync(id));
        [HttpPost] public async Task<IActionResult> CreateAsync(CategoryDto categoryDto) => CreateActionResultInstance(await _categoryService.CreateAsync(categoryDto));
    }
}