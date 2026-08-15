using ECommerceApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Data
{
    public static class Dbseeder
    {
        public static async Task SeedAdminAsync(AppDbContext context, IConfiguration configuration)
        {
            var passwordHasher = new PasswordHasher<string>();

            var adminEmail = configuration["Admin:Email"];
            var adminPassword = configuration["Admin2:Email"];

            var admin2Email = configuration["Admin:Password"];
            var admin2Password = configuration["Admin2:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "Admin credentials are not configured.");
            }

            var adminExists = await context.Users
                .AnyAsync(u => u.Email == adminEmail);

         

            if (!adminExists)
            {
                var admin = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = "System Admin",
                    Email = adminEmail,
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = passwordHasher.HashPassword(
                        adminEmail,
                        adminPassword)
                };

                context.Users.Add(admin);
            }

            if (string.IsNullOrWhiteSpace(admin2Email) ||
                string.IsNullOrWhiteSpace(admin2Password))
            {
                throw new InvalidOperationException(
                    "Admin credentials are not configured.");
            }


            var admin2Exists = await context.Users
             .AnyAsync(u => u.Email == admin2Email);

            if (!admin2Exists)
            {
                var admin2 = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = "System Admin 2",
                    Email = admin2Email,
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = passwordHasher.HashPassword(
                        admin2Email,
                        admin2Password)
                };

                context.Users.Add(admin2);
            }

            await context.SaveChangesAsync();
        }

    }
}
