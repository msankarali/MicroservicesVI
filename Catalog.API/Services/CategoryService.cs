using AutoMapper;
using Catalog.API.Configurations;
using Catalog.API.Dtos;
using Catalog.API.Models;
using MongoDB.Driver;
using Utilities.Results;

namespace Catalog.API.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(c => true).ToListAsync();

            return ApiResponse<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<ApiResponse<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _categoryCollection.InsertOneAsync(category);

            return ApiResponse<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<ApiResponse<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(c => c.Id == id).FirstOrDefaultAsync();

            if(category is null)
            {
                return ApiResponse<CategoryDto>.Fail(404, "Category not found");
            }

            return ApiResponse<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}