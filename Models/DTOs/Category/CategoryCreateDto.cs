using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }

    }
}
