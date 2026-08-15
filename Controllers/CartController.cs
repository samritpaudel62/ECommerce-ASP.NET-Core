using ECommerceApi.Models.DTOs.CartItem;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;

namespace ECommerceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(CartService cartService) : ControllerBase
    {
        //----------Add  Product To Cart----------
        [HttpPost("items")]
        public async Task<IActionResult> AddToCart(CartItemCreateDto dto)
        {
            var userId = GetUserId();

            if (userId == Guid.Empty)
                return Unauthorized();


            var cartItem = await cartService.AddToCartAsync(userId, dto);

            if (cartItem == null)
            {
                return BadRequest(
                    "Product not found or request quantity is invalid");
            }
            return CreatedAtAction(
                nameof(GetCartItemByProductId),
                new { productId = cartItem.ProductId }, cartItem);
                
        }

        //----------Get Cart Item of a user----------
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await cartService.GetAllCartItemsAsync(userId);

            return Ok(result);
        }


        //----------Remove a Product from the cart----------

        [HttpDelete("{productId:guid}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await cartService.RemoveItemFromCartAsync(userId, productId);

            if (!result)
            {
                return BadRequest(
                    "Unable to Remove the item");
            }

            return NoContent();
        }

        //--------Update the quantity Of the item in a cart ----------
        [HttpPut("{productId:guid}/{qty:int}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute] int qty, [FromRoute] Guid productId)
        {
            var userId = GetUserId();

            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await cartService.UpdateCartQuantityAsync(qty, userId, productId);

            if (result == null)
                return BadRequest("Product or quantity is invalid ");
            return Ok(result);// return NoContent()

        }

        //----------Get A CartItem----------
        [HttpGet("{productId:guid}")]

        public async Task<IActionResult> GetCartItemByProductId([FromRoute] Guid productId )
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await cartService.GetCartItemByIdAsync(productId, userId);

            if (result == null)
                return BadRequest("Product invalid");
            return Ok(result);
        }



        //----------Get UserClaim Principle----------
        private Guid GetUserId() {

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);


            if (userIdClaim == null)
              return Guid.Empty;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Guid.Empty;

            return userId;
        }
    }
}
