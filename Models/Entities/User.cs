namespace ECommerceApi.Models.Entities
{
    public class User
    {
        public  Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
        public  DateTime CreatedAt { get; set; }


        //adding navigation properties
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<CartItem> CartItems { get; set; } = [];
    }

}
