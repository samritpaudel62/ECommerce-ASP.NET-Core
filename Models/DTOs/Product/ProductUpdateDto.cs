using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.Product
{
    public class ProductUpdateDto
    {
        [Required]
        [StringLength(200)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Range(0.01,double.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
