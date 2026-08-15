namespace ECommerceApi.Models.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0.0m;
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

        public byte[] RowVersion { get; set; } = [];



        //adding the navigating properties
        public  Category? Category { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<CartItem> CartItems { get; set; } = [];
          

        public ICollection<OrderItem> OrderItems { get; set; } = [];
            

    }
}
