namespace ECommerceApi.Models.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        //adding navigation properties

        public  User User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = [];


    }
}   
