using Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands;
using Order.Application.Dtos;
using Order.Application.Queries;
using Utilities.Identity;
using Utilities.Results;

namespace Order.API.Controllers
{
    [Route("api/[controller]/[actions]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _sharedIdentityService = sharedIdentityService ?? throw new ArgumentNullException(nameof(sharedIdentityService));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync() =>
            CreateActionResultInstance(
                await _mediator.Send(new GetOrdersByUserIdQuery
                {
                    UserId = _sharedIdentityService.GetUserId
                })
            );

        [HttpPost]
        public async Task<IActionResult> SaveOrderAsync(CreateOrderCommand createOrderCommand)
        {
            var response = await _mediator.Send(createOrderCommand);

            return CreateActionResultInstance(response);
        }
    }
}