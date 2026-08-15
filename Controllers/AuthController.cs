using ECommerceApi.Models.DTOs.Auth;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await authService.GetUsersAsync();
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var result = await authService.GetUserByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            var result = await authService.RegisterUserAsync(registerDto);
            if (result == null)
                return BadRequest();

            return CreatedAtAction(
                nameof(GetUser),
                new { id = result.UserId },
                  result);
        }

        // login process

        [HttpPost("login")]
       
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await authService.LoginUserAsync(loginDto);
            if(result == null)
            {
                return Unauthorized();
            }
           return Ok(result);
        }
    }
}
    
