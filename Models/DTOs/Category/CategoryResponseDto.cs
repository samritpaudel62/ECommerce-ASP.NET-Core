using ECommerceApi.Models.DTOs.Product;

namespace ECommerceApi.Models.DTOs.Category
{
    public class CategoryResponseDto
    {
        public Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }



        public List<CategoryProductResponseDto> Products { get; set; } = [];
    }
}
