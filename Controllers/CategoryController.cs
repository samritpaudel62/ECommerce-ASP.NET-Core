using ECommerceApi.Models.DTOs.Category;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(CategoryService categoryService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await categoryService.LisAllCategory();

            return Ok(categories);

        }

        [HttpGet("{cateogryId:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid cateogryId)
        {
            var category = await categoryService.GetCategoryById(cateogryId);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId");
            }
            return Ok(category);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
        {
            var result = await categoryService.CategoryCreateAsync(dto);
            return CreatedAtAction(
                nameof(GetCategoryById),
                new { cateogryId = result.CategoryId },
                result);

        }
        [Authorize(Roles="Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryUpdateDto dto)
        {
            var result =  await categoryService.UpdateCategoryAsync(id, dto);
            if (result == null)
            {
                return BadRequest("Invaild ");
            }
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public   async Task<IActionResult> DeleteCategory(Guid id)
        {

            if (!await categoryService.DeleteCategoryAsync(id))
                return BadRequest("Invalid");
            return NoContent();
        }

    }
}