using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseProductRequest
    {
        /// <summary>
        /// Category of product
        /// </summary>
        [Required]
        public string CategoryId { get; set; }

        /// <summary>
        /// Brand of product.
        /// </summary>
        [Required]
        public string BrandId { get; set; }

        /// <summary>
        /// Name of product | Rules : MaxLenght 50
        /// </summary>      
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Short  Description of product. | Rules : MaxLenght 500
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Feature of product.| Rules : MaxLenght 50
        /// </summary>     
        [Required]
        public List<string> Feature { get; set; }

        /// <summary>
        /// Crytal of product.| Rules : MaxLenght 50
        /// </summary>     
        [Required]
        public string Crytal { get; set; }

        /// <summary>
        /// Machine of product.| Rules : MaxLenght 50
        /// </summary>     
        [Required]
        public string Machine { get; set; }

        /// <summary>
        /// Guarantee of product.| Rules : MaxLenght 50
        /// </summary>     
        public int Guarantee { get; set; }

        /// <summary>
        /// Ablert  of product.
        /// </summary>     
        [Required]
        public string Ablert { get; set; }

        /// <summary>
        /// WaterProof.
        /// </summary>     
        [Required]
        public string WaterProof { get; set; }

        /// <summary>
        /// MadeIn of product.| Rules : MaxLenght 50
        /// </summary>     
        [Required]
        public string MadeIn { get; set; }

        /// <summary>
        /// Thumbnail image of product.
        /// </summary>
        [Required]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Active product.
        /// </summary>
        [Required]
        public int IsShow { get; set; }
    }
}
