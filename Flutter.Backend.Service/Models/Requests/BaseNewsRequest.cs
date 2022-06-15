using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseNewsRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Thumbnail { get; set; }

        public int IsShow { get; set; }
    }
}
