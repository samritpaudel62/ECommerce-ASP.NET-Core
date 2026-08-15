using ECommerceApi.Exceptions;
using ECommerceApi.Models.DTOs;
using ECommerceApi.Models.DTOs.Order;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;

namespace ECommerceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderService orderService) : ControllerBase
    {

        [HttpPost("checkout")]
        public async Task<IActionResult> OrderCheckOut()
        {
            try
            {
                var userId = GetUserId();
                if (userId == Guid.Empty)
                    return Unauthorized();

                var order = await orderService.CheckoutOrderAsync(userId);

                if (order == null)
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        StatusCode = 400,
                        Message = "Invalid order status."
                    });

                }
                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { orderId = order.OrderId },
                    order);
            }
            catch (StockConcurrencyException ex)
            {
                return Conflict(new
                {
                    message = ex.Message,
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderQueryDto query)
        {
            var orders = await orderService.GetAllOrdersAsync(query);
            return Ok(orders);
        }
        //------- get my orders-------

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = GetUserId();

            if (userId == Guid.Empty)
                return Unauthorized(
                    new ApiErrorResponse
                    {
                        StatusCode = 401,
                        Message = "Authentication is Required"
                    });


            var orders = await orderService.GetMyOrderAsync(userId);

            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var order = await orderService.GetMyOrderByIdAsync(userId, orderId);

            if (order == null)
            {
                return NotFound(new ApiErrorResponse
                {
                    StatusCode = 404,
                    Message = "Order Not Found"
                });
            }
            return Ok(order);

        }

        [HttpGet("admin/{orderId:guid}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetOrderByIdForAdmin([FromRoute] Guid orderId)
        {
            var order = await orderService.GetAnyOrderByAdminAsync(orderId);
            if (order == null)
                return BadRequest(new ApiErrorResponse
                {
                    StatusCode = 400,
                    Message = "Invalid Order Status."
                });

            return Ok(order);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId:Guid}/{status}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid orderId, [FromBody] OrderStatusUpdateDto status)
        {
            var result = await orderService.UpdateOrderStatusAsync(orderId, status);


            return result.StatusCode switch
            {
                200 =>
                Ok(result.Order),

                400 =>
                BadRequest(new ApiErrorResponse
                {
                    StatusCode = 400,
                    Message = "Invalid order status."
                }),

                404 =>
                NotFound(new ApiErrorResponse
                {
                    StatusCode = 404,
                    Message = "Order not found."
                }),

                409 =>
                Conflict(new ApiErrorResponse
                {
                    StatusCode = 409,
                    Message = "Conflict"
                }),

                _ =>
                BadRequest("Unable to update order status.")
            };
        }

        private Guid GetUserId()
        {
            var userClaims = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userClaims == null)
                return Guid.Empty;
            if (!Guid.TryParse(userClaims, out var userId))
                return Guid.Empty;

            return userId;
        }


    }
}
