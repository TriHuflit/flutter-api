using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseClassifyProductRequest
    {

        /// <summary>
        /// Image
        /// </summary>
        [Required]
        public string Image { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// OriginalPrice
        /// </summary>
        [Required]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// PromotionPrice
        /// </summary>
        [Required]
        public decimal PromotionPrice { get; set; }

        /// <summary>
        /// Stock
        /// </summary>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// IsShow
        /// </summary>
        [Required]
        public int IsShow { get; set; }
    }
}
