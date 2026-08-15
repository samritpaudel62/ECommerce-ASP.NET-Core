using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.CartItem
{
    public class CartItemCreateDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(0.01, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
