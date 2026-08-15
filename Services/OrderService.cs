
using ECommerceApi.Data;
using ECommerceApi.Exceptions;
using ECommerceApi.Models.DTOs.Order;
using ECommerceApi.Models.DTOs.OrderItem;
using ECommerceApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ECommerceApi.Services
{
    public class OrderService(AppDbContext context)
    {
        //public enum OrderStatusUpdateResult
        //{
        //    Success,
        //    OrderNotFound,
        //    InvalidStatus,
        //    InvalidTransition
        //}
        public async Task<OrderResponseDto?> CheckoutOrderAsync(Guid userId)
        {
            var cartItems = await context.CartItems
                .Include(p => p.Product)
                .Where(u => u.UserId == userId).ToListAsync();

            if (cartItems.Count() == 0)
                return null;

            await using var transaction =
                await context.Database.BeginTransactionAsync();


            try
            {

                foreach (var item in cartItems)
                {
                    if (item.Product.StockQuantity < item.Quantity)
                        return null;
                }
                var totalAmount = cartItems.Sum(
                    item => item.Product.Price * item.Quantity);


                var order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = userId,
                    TotalAmount = totalAmount,
                    Status = "pending",
                    CreatedAt = DateTime.UtcNow,
                };

                await context.AddAsync(order);
                List<OrderItem> orderItems = [];

                foreach (var item in cartItems)
                {
                    var  orderItem = new OrderItem
                    {
                        OrderItemId = Guid.NewGuid(),
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    };

                    context.OrderItems.Add(orderItem);
                    orderItems.Add(orderItem);
                    item.Product.StockQuantity -= item.Quantity;
                }

                context.CartItems.RemoveRange(cartItems);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();


                return new OrderResponseDto
                {
                    OrderId = order.OrderId,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    CreatedAt = order.CreatedAt,
                    UserId = order.UserId,
                    OrderItems = orderItems
                    .Select(items => new OrderItemResponseDto()
                    {
                        Quantity = items.Quantity,
                        ProductId = items.ProductId,
                        OrderItemId = items.OrderItemId,
                        UnitPrice = items.UnitPrice,
                    }).ToList(),
                };
            }
            catch(DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
               throw new StockConcurrencyException();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }


        }



        public async Task<List<OrderResponseDto>> GetMyOrderAsync(Guid userId)
        {
            var orders = await context.Orders
                .Include(o => o.OrderItems)
                .Where(x => x.UserId == userId)
                .OrderByDescending(o => o.CreatedAt).ToListAsync();

            var orderResponseDto = orders.Select(order => new OrderResponseDto()
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems
                .Select(item => new OrderItemResponseDto()
                {
                    OrderItemId = item.OrderItemId,
                    ProductId = item.ProductId,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();


            return orderResponseDto;
        }

        public async Task<OrderResponseDto?> GetMyOrderByIdAsync(Guid userId, Guid orderId)
        {
            var order = await context.Orders.Include(o => o.OrderItems).Where(o =>
            o.UserId == userId && o.OrderId == orderId)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            return new OrderResponseDto()
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems
                    .Select(item => new OrderItemResponseDto()
                    {
                        OrderItemId = item.OrderItemId,
                        ProductId = item.ProductId,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                    }).ToList(),
                Status = order.Status,
            };

        }


        public async Task<OrderResponseDto?> GetAnyOrderByAdminAsync(Guid orderId)
        {
            var order = await context.Orders.Include(o=> o.OrderItems).FirstOrDefaultAsync(x =>
            x.OrderId == orderId);

            if (order == null)
                return null;

            return new OrderResponseDto()
            {
                OrderId = order.OrderId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UserId = order.UserId,
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDto()
                {
                    OrderItemId = item.OrderItemId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice


                }).ToList()
            };






        }

        public async Task<OrderPagedResponseDto> GetAllOrdersAsync(OrderQueryDto query)
        {
            if (query.Page < 1)
                query.Page = 1;
            if (query.PageSize < 1)
                query.PageSize = 10;

            if (query.PageSize > 100)
                query.PageSize = 10;


            var orderQuery = context.Orders
                .Include(o => o.OrderItems)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                orderQuery = orderQuery.Where(o =>
                o.Status == query.Status);
            }


            var totalItems = await orderQuery.CountAsync();

            var orders = await orderQuery
                .OrderByDescending(o => o.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(order => new OrderResponseDto
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    CreatedAt = order.CreatedAt,

                    OrderItems = order.OrderItems
                    .Select(item => new OrderItemResponseDto
                    {
                        OrderItemId = item.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    }).ToList(),

                }).ToListAsync();



            return new OrderPagedResponseDto
            {
                Items = orders,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItmes = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)



            };


        }


        public async Task<(int StatusCode,OrderResponseDto? Order)>
            UpdateOrderStatusAsync(Guid orderId, OrderStatusUpdateDto status)
        {
            var order = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return (404, null);

            var allowedStatus = new[]
            {
                "Pending",
                "Processing",
                "Shipped",
                "Delivered",
                "Cancelled"
            };
            if (!allowedStatus.Contains(status.Status))
                return (400, null);

            var allowedTransitions = new Dictionary<string, string[]>
            {
                ["pending"] = ["Pending"],
                ["Pending"] = ["Pending","Processing", "Cancelled"],
                ["Processing"] = ["Shipped", "Cancelled"],
                ["Shipped"] = ["Delivered"],
                ["Delivered"] = [],
                ["Cancelled"] = []
            };

            if (!allowedTransitions.TryGetValue(
                order.Status,
                out var allowedNextStatuses))

            {
                return (409,null);
            }

            if (!allowedNextStatuses.Contains(status.Status))
                return (409, null);

            order.Status = status.Status;

            await context.SaveChangesAsync();

            var response = new OrderResponseDto
            {
                Status = order.Status,
                OrderId = order.OrderId,

                OrderItems = order.OrderItems
                    .Select(item => new OrderItemResponseDto
                    {
                        OrderItemId = item.OrderItemId,
                        ProductId = item.ProductId,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    })
                    .ToList(),

                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt
            };

            return (200, response);
        }
    }
}
