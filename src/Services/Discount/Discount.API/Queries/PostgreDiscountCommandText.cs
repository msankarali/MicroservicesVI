namespace Discount.API.Queries
{
    public class PostgreDiscountCommandText : IDiscountCommandText
    {
        public string GetAll => "SELECT * FROM discount";
        public string GetById => "SELECT * FROM discount WHERE id=@Id";
        public string Add => "INSERT INTO discount (user_id, rate, code) VALUES (@UserId, @Rate, @Code)";
        public string Update => "UPDATE discount SET user_id=@UserId, rate=@Rate, code=@Code WHERE id=@Id";
        public string DeleteById => "DELETE FROM discount WHERE id=@Id";
        public string GetByCodeAndUserId => "SELECT * FROM discount WHERE user_id=@UserId AND code=@Code";
    }
}