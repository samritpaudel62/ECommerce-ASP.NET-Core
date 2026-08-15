namespace ECommerceApi.Models.DTOs.Product
{
    public class ProductResponseDto
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public string?  Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
