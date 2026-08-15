using ECommerceApi.Data;
using ECommerceApi.Models.DTOs.Product;
using ECommerceApi.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class ProductService(AppDbContext context)
    {
        public async Task<ProductPagedResponseDto> GetAllProductsAsync(ProductQueryDto query)
        {
            if(query.Page < 1)
                query.Page = 1;

            if(query.PageSize < 1)
                query.PageSize = 10;

            if(query.PageSize > 100)
                query.PageSize = 100;

            var productsQuery = context.Products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                productsQuery = productsQuery.Where(p =>
                p.Name.Contains(query.Search));
            }

            if(query.CategoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p =>
                p.CategoryId == query.CategoryId.Value);
            }

            var totalItems = await productsQuery.CountAsync();

            var products = await productsQuery
                .OrderBy(p => p.Name)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new ProductResponseDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CreatedAt = p.CreatedAt,
                    CategoryId = p.CategoryId,
                }).ToListAsync();

            return new ProductPagedResponseDto
            {
                Items = products,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)query.PageSize)

            };
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid id)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (product is null)
                return null;

            var productResponseDto = new ProductResponseDto()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive

            };
            return productResponseDto;
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto productDto)
        {


            Product product = new Product()
            {
                ProductId = Guid.NewGuid(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                ImageUrl = productDto.ImageUrl,
                StockQuantity = productDto.StockQuantity,
                CategoryId = productDto.CategoryId,
                CreatedAt = DateTime.UtcNow,
            };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return new ProductResponseDto()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CreatedAt = DateTime.UtcNow,
                CategoryId = product.CategoryId,
            };

        }

        public async Task<ProductResponseDto?> UpdateProductAsync(Guid id,  ProductUpdateDto dto)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;

            await context.SaveChangesAsync();

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive
            };
        }
        public async Task<ProductResponseDto?> UpdateProductStatusAsync( Guid id, bool isActive)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return null;

            product.IsActive = isActive;

            await context.SaveChangesAsync();

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive
            };
        }


        public async Task<ProductResponseDto?> UpdateStockAsync(Guid productId , StockUpdateDto dto)
        {
           if(dto.StockQuantity <= 0) return null;

           var product = await context.Products.FirstOrDefaultAsync(p=> p.ProductId == productId);

            if(product == null)     return null;


            product.StockQuantity = dto.StockQuantity;
            await context.SaveChangesAsync();

            return new ProductResponseDto()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive

            };
        }
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
                return false;
            else
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();

                return true;
            }
        }
    }
}
