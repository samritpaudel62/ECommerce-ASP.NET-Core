using System.Reflection.Metadata.Ecma335;

namespace ECommerceApi.Models.DTOs.Product
{
    public class ProductQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
