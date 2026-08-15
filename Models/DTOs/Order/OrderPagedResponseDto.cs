namespace ECommerceApi.Models.DTOs.Order
{
    public class OrderPagedResponseDto
    {
        public List<OrderResponseDto> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItmes { get; set; }
        public int TotalPages { get; set; }
    }
}
