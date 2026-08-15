namespace ECommerceApi.Models.DTOs.OrderItem
{
    public class OrderItemResponseDto
    {
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice{ get; set; }
        
    }
}
