using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [PasswordPropertyText]
        public required string Password { get; set; }
        

    }
}
