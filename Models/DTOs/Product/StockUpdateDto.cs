using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models.DTOs.Product
{
    public class StockUpdateDto
    {
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}
