using ECommerceApi.Data;
using ECommerceApi.Models.DTOs.Auth;
using ECommerceApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceApi.Services
{
    
    public class AuthService(AppDbContext context, IConfiguration configuration)
    {

        private static string PassWordHashing(string email, string password)
        {
            var passwordHasher = new PasswordHasher<string>();

            var hash = passwordHasher.HashPassword(email, password);
            return hash;
        }



        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);



            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //registration operations
       
        public async Task<List<LoginResponseDto>> GetUsersAsync()
        {
            var users = await context.Users.ToListAsync();

            var loginResponseDtos = users.Select(u => new LoginResponseDto()
            {
                UserId = u.UserId,
                Email = u.Email,
                Name = u.Name,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();
            return loginResponseDtos;
        }
      
        public async Task<LoginResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                return null;
            }

            var loginResponseDto = new LoginResponseDto()
            {
                Email = user.Email,
                Name = user.Name,
                UserId = user.UserId,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
            return loginResponseDto;
        }

        public async Task<LoginResponseDto?> RegisterUserAsync(RegisterDto dto)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingUser != null)
            {
                return null;
            }


            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = PassWordHashing(dto.Email, dto.Password),
                CreatedAt = DateTime.UtcNow,
                Role = "Customer"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new LoginResponseDto()
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Role = user.Role
            };
        }

        /// login operations

        public async Task<LoginResponseDto?> LoginUserAsync(LoginDto dto)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (existingUser == null)
            {
                return null;
            }
            else
            {
                var passwordHasher = new PasswordHasher<string>();

                var result = passwordHasher.VerifyHashedPassword(
                    existingUser.Email,
                    existingUser.PasswordHash,
                    dto.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return null;
                }

                if (result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    existingUser.PasswordHash = passwordHasher.HashPassword(
                        existingUser.Email,
                        dto.Password);

                    await context.SaveChangesAsync();
                }

                return new LoginResponseDto()
                {
                    Email = existingUser.Email,
                    Name = existingUser.Name,
                    CreatedAt = existingUser.CreatedAt,
                    Role = existingUser.Role,
                    UserId = existingUser.UserId,
                    Token = GenerateJwtToken(existingUser)
                };
            }



        }

    }

}



