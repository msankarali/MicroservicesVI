namespace Discount.API.Queries
{
    public interface IDiscountCommandText
    {
        string GetAll { get; }
        string GetById { get; }
        string Add { get; }
        string Update { get; }
        string DeleteById { get; }
        string GetByCodeAndUserId { get; }
    }
}