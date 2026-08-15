using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [PasswordPropertyText]
        public required  string Password { get; set; }
    }
}
