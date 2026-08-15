using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.Entities
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        
        public int Quantity { get; set; }


        //adding navigation properties

        public  User User { get; set; }
        public Product Product { get; set; } 

    }
}
