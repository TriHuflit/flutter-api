using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseBannerRequest
    {
        [Required]
        public string ImageBanner { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string IdProduct { get; set; }

        public int IsShowOnMobile { get; set; }
    }
}
