using ECommerceApi.Data;
using ECommerceApi.Models.DTOs.CartItem;
using ECommerceApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Quic;

namespace ECommerceApi.Services
{
    
    public class CartService(AppDbContext context)
    {
        
        //----------Add  Product To Cart----------
        public async Task<CartItemResponseDto?> AddToCartAsync(Guid userId, CartItemCreateDto dto)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);
            // validating the products exists or not and the stockquantity
            if (product == null)
            {
                return null;
            }

            if (dto.Quantity <= 0)
                return null;

            if (dto.Quantity > product.StockQuantity)
                return null;


            var existingItem = await context.CartItems
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    c.ProductId == dto.ProductId);


            // if product is already in the cart with some quantity
            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + dto.Quantity;

                if (newQuantity > product.StockQuantity)
                    return null;

                existingItem.Quantity = newQuantity;
                await context.SaveChangesAsync();

                return new CartItemResponseDto()
                {
                    Quantity = existingItem.Quantity,
                    ProductId = existingItem.ProductId,
                    CartItemId = existingItem.CartItemId,

                };
            }
           // if product is not in the cart and have to add a new product in the cart
                var cartItem = new CartItem()
                {
                    CartItemId = Guid.NewGuid(),
                    UserId = userId,
                    Quantity = dto.Quantity,
                    ProductId = dto.ProductId,

                };
                context.CartItems.Add(cartItem);

            await context.SaveChangesAsync();

            return new CartItemResponseDto()
            {
                CartItemId = cartItem.CartItemId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,

            };
        }


        //----------Get Cart Items of a user----------
        public async Task<List<CartItemResponseDto>?> GetAllCartItemsAsync(Guid userId)
        {
            var cartItems = await context.CartItems.Where(x => x.UserId == userId).ToListAsync();

            var cartItemsResponseDto = cartItems.Select(x => new CartItemResponseDto()
            {
                CartItemId = x.CartItemId,
                ProductId= x.ProductId,
                Quantity= x.Quantity,
            }).ToList();


            return cartItemsResponseDto;
        }


        //----------Remove a Product from the cart----------
        public async Task<bool> RemoveItemFromCartAsync( Guid userId,  Guid productId)
        {
            var cartItem = context.CartItems.FirstOrDefault(x => x.ProductId == productId && x.UserId == userId );
            if (cartItem == null)
                return false;
       
            var product = context.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product == null)
                return false;


             context.Remove(cartItem);
            await context.SaveChangesAsync();
            return true; 



        }


        //--------Update the quantity Of the item in a cart ----------
        public async Task<CartItemResponseDto?> UpdateCartQuantityAsync(int quantity , Guid userId , Guid productId)
        {
            var cartItem = await context.CartItems.FirstOrDefaultAsync(x=> x.UserId == userId && x.ProductId == productId);
            if (quantity < 0)
                return null;

            if (cartItem == null)
                return null;

            var product = await context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
                return null;


            if (quantity > product.StockQuantity)
                return null;

            cartItem.Quantity = quantity;

            await context.SaveChangesAsync();

            return new CartItemResponseDto()
            { 
                CartItemId = cartItem.CartItemId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };

        }


        //----------Get A CartItem----------

        public async Task<CartItemResponseDto?> GetCartItemByIdAsync( Guid productId, Guid userId )
        {
            var cartItem = await context.CartItems.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
            if( cartItem == null )
                return null;

            return new CartItemResponseDto()
            {
                ProductId = cartItem.ProductId,
                CartItemId = cartItem.CartItemId,
                Quantity = cartItem.Quantity
            };

        }

    }




    
}
