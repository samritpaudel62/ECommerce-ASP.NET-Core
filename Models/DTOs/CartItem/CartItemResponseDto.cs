namespace ECommerceApi.Models.DTOs.CartItem
{
    public class CartItemResponseDto
    {
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }


    }
}
