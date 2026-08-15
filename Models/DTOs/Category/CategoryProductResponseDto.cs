using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.Category
{
    public class CategoryProductResponseDto
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }




    }
}
