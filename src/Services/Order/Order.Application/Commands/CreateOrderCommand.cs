using MediatR;
using Order.Application.Dtos;
using Order.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Results;

namespace Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<ApiResponse<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public AddressDto Address { get; set; }

        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<CreatedOrderDto>>
        {
            private readonly OrderDbContext _orderDbContext;

            public CreateOrderCommandHandler(OrderDbContext orderDbContext)
            {
                _orderDbContext = orderDbContext;
            }

            public async Task<ApiResponse<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var newAddress = new Domain.OrderAggregate.Address(
                    province: request.Address.Province,
                    district: request.Address.District,
                    street: request.Address.Street,
                    zipCode: request.Address.ZipCode,
                    line: request.Address.Line);

                var newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

                request.OrderItems.ForEach(oi =>
                {
                    newOrder.AddOrderItem(
                        productId: oi.ProductId,
                        productName: oi.ProductName,
                        price: oi.Price,
                        pictureUrl: oi.PictureUrl);
                });

                _orderDbContext.Orders.Add(newOrder);

                await _orderDbContext.SaveChangesAsync(cancellationToken);

                return ApiResponse<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
            }
        }
    }
}