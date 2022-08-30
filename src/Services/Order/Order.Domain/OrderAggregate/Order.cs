using DDD.Core;

namespace Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private Order() { }

        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; } //Owned Entity Type - columns in Order table or another table with columns
        public string BuyerId { get; private set; } //Shadow property

        private readonly List<OrderItem> _orderItems; //Backing field - encapsulation, can be taken but should not be set. EF Core fills it
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems; //opening it

        public Order(string buyerId, Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItems.Any(oi => oi.ProductId == productId);

            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);

                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(oi => oi.Price);
    }
}