using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoProduct : DtoAuditLogSystem
    {
        /// <summary>
        /// Id of product.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Category of product
        /// </summary>
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        /// <summary>
        /// Brand of product.
        /// </summary>

        public string BrandID { get; set; }
        public string BrandName { get; set; }
        /// <summary>
        /// Name of product | Rules : MaxLenght 50
        /// </summary>      
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
        /// Feature of product
        /// </summary>     
        public List<string> Feature { get; set; }

        /// <summary>
        /// Short  Description of product. | Rules : MaxLenght 50
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Thumbnail image of product.
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Machine of product.
        /// </summary>     
        public string Machine { get; set; }

        /// <summary>
        /// Active product.
        /// </summary>
        public int? IsShow { get; set; }
    }
}
