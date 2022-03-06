using MongoDB.Bson;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.DAL.Domains
{
    public class Product : AuditLogSystem
    {
        /// <summary>
        /// ID of product.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// IDCategory of product.
        /// </summary>
        [Required]
        public ObjectId CategoryID { get; set; }

        /// <summary>
        /// IDBrand of product.
        /// </summary>
        [Required]
        public ObjectId BrandID { get; set; }

        /// <summary>
        /// Name of product.| Rules : MaxLenght 50
        /// </summary>
        [Required]
        [MaxLength (50)]
        public string Name { get; set; }

        /// <summary>
        /// Price at least of product.
        /// </summary>
        public decimal FromPrice { get; set; }

        /// <summary>
        /// Price at most of product.
        /// </summary>    
        public decimal ToPrice { get; set; }

        /// <summary>
        /// Short  Description of product. | Rules : MaxLenght 50
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Thumbnail image of product.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Active product.
        /// </summary>
        [Required]
        public bool IsShow { get; set; }
    }
}
