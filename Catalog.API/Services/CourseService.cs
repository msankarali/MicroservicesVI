using AutoMapper;
using Catalog.API.Configurations;
using Catalog.API.Dtos;
using Catalog.API.Models;
using MongoDB.Driver;
using Utilities.Results;

namespace Catalog.API.Services
{
    internal class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(c => true).ToListAsync() ?? new List<Course>();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(c => c.Id == course.CategoryId).FirstAsync();
                }
            }

            return ApiResponse<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ApiResponse<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(c => c.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return ApiResponse<CourseDto>.Fail(404, "Course not found");
            }

            course.Category = await _categoryCollection.Find(c => c.Id == course.CategoryId).FirstAsync();

            return ApiResponse<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<ApiResponse<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(c => c.UserId == userId).ToListAsync() ?? new List<Course>();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(c => c.Id == course.CategoryId).FirstAsync();
                }
            }

            return ApiResponse<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ApiResponse<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedDate = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse);

            return ApiResponse<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<ApiResponse<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);

            var result = await _courseCollection.FindOneAndReplaceAsync(c => c.Id == courseUpdateDto.Id, updateCourse);

            if(result == null)
            {
                return ApiResponse<NoContent>.Fail(404, "Course not found");
            }

            return ApiResponse<NoContent>.Success(204);
        }

        public async Task<ApiResponse<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(c=>c.Id == id);

            if(result.DeletedCount > 0)
            {
                return ApiResponse<NoContent>.Success(204);
            }

            return ApiResponse<NoContent>.Fail(404, "Course not found");
        }
    }
}