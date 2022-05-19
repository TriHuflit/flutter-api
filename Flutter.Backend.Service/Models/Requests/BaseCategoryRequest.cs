using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseCategoryRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageCategory { get; set; }

        [Required]
        public int IsShow { get; set; }

    }
}
