using ECommerceApi.Data;
using ECommerceApi.Models.DTOs.Category;
using ECommerceApi.Models.DTOs.Product;
using ECommerceApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace ECommerceApi.Services
{
    public class CategoryService(AppDbContext context)
    {
        public async Task<CategoryResponseDto> CategoryCreateAsync(CategoryCreateDto dto)
        {
            var category = new Category()
            {
                CategoryId = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description

            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new CategoryResponseDto()
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description

            };
        }


        public async Task<List<CategoryResponseDto>> LisAllCategory()
        {
            var categories = await context.Categories.Include(p => p.Products).ToListAsync();// IQueryable

            var categoryResponseDto = categories.Select(category => new CategoryResponseDto()
            {
                CategoryId = category.CategoryId,
                Description = category.Description,
                Name = category.Name,
                Products = category.Products.Select(item => new CategoryProductResponseDto()
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Description = item.Description,
                    ImageUrl = item.ImageUrl,
                    Price = item.Price,
                }).ToList()
            }).ToList(); // to List when sending to API or controller

            return categoryResponseDto;
        }

        public async Task<CategoryResponseDto?> GetCategoryById(Guid id)
        {
            var category = await context.Categories.Include(c => c.Products).FirstOrDefaultAsync(x => x.CategoryId == id);
            if (category == null)
                return null;

            return new CategoryResponseDto()
            {
                CategoryId = category.CategoryId,
                Description = category.Description,
                Name = category.Name,
                Products = category.Products.Select(item => new CategoryProductResponseDto()
                {
                    Name = item.Name,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Description = item.Description,
                    ImageUrl = item.ImageUrl,

                }).ToList()
            };
        }

        public async Task<CategoryResponseDto?> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return null;

            category.Name = dto.Name;
            category.Description = dto.Description;

            await context.SaveChangesAsync();

            return new CategoryResponseDto
            {
                Name = category.Name,
                Description = category.Description,
                CategoryId = category.CategoryId,
                
            };

        }
        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return false;

            context.Categories.Remove(category);
            await context.SaveChangesAsync();


            return true;
        }
    }

}
