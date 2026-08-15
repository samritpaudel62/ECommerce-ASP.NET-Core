namespace ECommerceApi.Models.DTOs.Auth
{
    public class LoginResponseDto
    {
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
