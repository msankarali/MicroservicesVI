using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Dtos;
using Order.Application.Mappings;
using Order.Infrastructure;
using Utilities.Results;

namespace Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<ApiResponse<List<OrderDto>>>
    {
        public string UserId { get; set; }

        public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, ApiResponse<List<OrderDto>>>
        {
            private readonly OrderDbContext _orderDbContext;

            public GetOrdersByUserIdQueryHandler(OrderDbContext orderDbContext)
            {
                _orderDbContext = orderDbContext;
            }

            public async Task<ApiResponse<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
            {
                var orders = await _orderDbContext.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.BuyerId == request.UserId).ToListAsync(cancellationToken);

                if (!orders.Any())
                {
                    return ApiResponse<List<OrderDto>>.Success(new List<OrderDto>(), 200);
                }

                var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

                return ApiResponse<List<OrderDto>>.Success(ordersDto, 200);
            }
        }
    }
}