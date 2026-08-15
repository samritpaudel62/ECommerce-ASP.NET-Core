namespace ECommerceApi.Models.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }


        //adding navigation properties

        public ICollection<Product> Products { get; set; }
          = [];
    }
}
