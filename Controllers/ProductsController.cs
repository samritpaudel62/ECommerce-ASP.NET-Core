

using ECommerceApi.Models.DTOs.Product;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ProductService productService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDto query)
        {
            var products = await productService.GetAllProductsAsync(query);

            return Ok(products);
        }


        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid Id)
        {


            var product = await productService.GetProductByIdAsync(Id);

            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto createDto)
        {
            var product = await productService.CreateProductAsync(createDto);


            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.ProductId },
                product);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductUpdateDto updateDto)
        {
            var updatedProduct = await productService.UpdateProductAsync(id, updateDto);

            if (updatedProduct is null)
                return NotFound(new { message = "Product not found" });

            return Ok(updatedProduct);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}/{status:bool}")]
        public async Task<IActionResult> UpdateProductStatus([FromRoute]Guid id, [FromRoute]bool status)
        {
            var updatedProduct = await productService.UpdateProductStatusAsync(id, status);

            if (updatedProduct is null)
                return NotFound(new { message = "Product not found." });
            return Ok(updatedProduct);

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}/stock")]
        public async Task<IActionResult> UpdateStock([FromRoute]Guid id, StockUpdateDto dto )
        {
            var product = await productService.UpdateStockAsync(id, dto);

            if (product is null)
                return BadRequest("Product not found or invalid stock quantity");
            return Ok(product);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct ([FromRoute] Guid id)
        {
            var deleted = await productService.DeleteProductAsync(id);

            if(!deleted)
                return NotFound();


            return NoContent();
        }
    }
}
