using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class CreateClassifyProductRequest
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
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// PromotionPrice
        /// </summary>
        public decimal PromotionPrice { get; set; }

        /// <summary>
        /// Stock
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// IsShow
        /// </summary>
        public int? IsShow { get; set; }
    }
}
