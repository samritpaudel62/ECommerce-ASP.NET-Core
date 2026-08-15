namespace ECommerceApi.Models.DTOs.Order
{
    public class OrderQueryDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Status { get; set; }
    }
}
