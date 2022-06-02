using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class CreateOrderDetailRequest
    {
        [Required]
        public string ClassifyProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ClassifyProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
