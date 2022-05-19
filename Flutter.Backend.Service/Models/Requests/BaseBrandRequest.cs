using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseBrandRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageBrand { get; set; }

        [Required]
        public int IsShow { get; set; }
    }
}
