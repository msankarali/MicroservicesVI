using Dapper;
using Discount.API.Queries;
using Npgsql;
using System.Data;
using Utilities.Results;

namespace Discount.API.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        private readonly IDiscountCommandText _discountCommandText;

        public DiscountService(IConfiguration configuration,
                               IDiscountCommandText discountCommandText)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
            _discountCommandText = discountCommandText;
        }

        public async Task<ApiResponse<NoContent>> DeleteByIdAsync(int id)
        {
            var rowsAffected = await _dbConnection.ExecuteAsync(_discountCommandText.DeleteById, new { Id = id });

            return rowsAffected > 0
                ? ApiResponse<NoContent>.Success(204)
                : ApiResponse<NoContent>.Fail(404, "Discount not found");
        }

        public async Task<ApiResponse<List<Entities.Discount>>> GetAllAsync()
        {
            var discountList = await _dbConnection.QueryAsync<Entities.Discount>(_discountCommandText.GetAll);

            return ApiResponse<List<Entities.Discount>>.Success(discountList.ToList(), 200);
        }

        public async Task<ApiResponse<Entities.Discount>> GetByCodeAndUserIdAsync(string code, string userId)
        {
            var discountList = await _dbConnection.QueryAsync<Entities.Discount>(_discountCommandText.GetByCodeAndUserId, new
            {
                Code = code,
                UserId = userId,
            });
            var discount = discountList.FirstOrDefault();

            return discount is null
                ? ApiResponse<Entities.Discount>.Fail(404, "Discount not found")
                : ApiResponse<Entities.Discount>.Success(discount, 200);
        }

        public async Task<ApiResponse<Entities.Discount>> GetByIdAsync(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Entities.Discount>(_discountCommandText.GetById, new { Id = id })).SingleOrDefault();

            if (discount is null)
            {
                return ApiResponse<Entities.Discount>.Fail(404, "Discount not found");
            }

            return ApiResponse<Entities.Discount>.Success(discount, 200);
        }

        public async Task<ApiResponse<NoContent>> AddAsync(Entities.Discount discount)
        {
            var rowsAffected = await _dbConnection.ExecuteAsync(_discountCommandText.Add, discount);

            if (rowsAffected > 0)
            {
                return ApiResponse<NoContent>.Success(200);
            }

            return ApiResponse<NoContent>.Fail(500, "An error occured while adding discount");
        }

        public async Task<ApiResponse<NoContent>> UpdateAsync(Entities.Discount discount)
        {

            var rowsAffected = await _dbConnection.ExecuteAsync(_discountCommandText.Update, new
            {
                Id = discount.Id,
                UserId = discount.UserId,
                Rate = discount.Rate,
                Code = discount.Code
            });

            if (rowsAffected > 0)
            {
                return ApiResponse<NoContent>.Success(204);
            }

            return ApiResponse<NoContent>.Fail(404, "Discount not found");
        }
    }
}