namespace ECommerceApi.Models.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        //adding navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}       
