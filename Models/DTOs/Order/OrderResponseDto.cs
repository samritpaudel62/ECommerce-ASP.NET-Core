using ECommerceApi.Models.DTOs.OrderItem;

namespace ECommerceApi.Models.DTOs.Order
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }


        public List<OrderItemResponseDto> OrderItems { get; set; } = [];
    }
}
