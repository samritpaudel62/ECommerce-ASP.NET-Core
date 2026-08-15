using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.Order
{
    public class OrderStatusUpdateDto
    {
        [Required]
        public required string  Status { get; set; } 
    }
}
