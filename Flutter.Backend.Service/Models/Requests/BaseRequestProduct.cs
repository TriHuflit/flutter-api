using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseRequestProduct
    {
        /// <summary>
        /// Category of product
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Brand of product.
        /// </summary>

        public string BrandId { get; set; }

        /// <summary>
        /// Name of product | Rules : MaxLenght 50
        /// </summary>      
        public string Name { get; set; }

        /// <summary>
        /// Short  Description of product. | Rules : MaxLenght 50
        /// </summary>

        public string Description { get; set; }

        /// <summary>
        /// Feature of product.| Rules : MaxLenght 50
        /// </summary>     
        public List<string> Feature { get; set; }

        /// <summary>
        /// Price at least of product.
        /// </summary>
        public decimal FromPrice { get; set; }

        /// <summary>
        /// Price at most of product.
        /// </summary>    
        public decimal ToPrice { get; set; }

        /// <summary>
        /// Crytal of product.| Rules : MaxLenght 50
        /// </summary>     
        public string CrytalId { get; set; }

        /// <summary>
        /// Machine of product.| Rules : MaxLenght 50
        /// </summary>     
        public string Machine { get; set; }

        /// <summary>
        /// Guarantee of product.| Rules : MaxLenght 50
        /// </summary>     
        public DateTime Guarantee { get; set; }

        /// <summary>
        /// Ablert  of product.
        /// </summary>     
        public string AblertId { get; set; }

        /// <summary>
        /// WaterProof.
        /// </summary>     
        public string WaterProofId { get; set; }

        /// <summary>
        /// MadeIn of product.| Rules : MaxLenght 50
        /// </summary>     
        public string MadeIn { get; set; }

        /// <summary>
        /// Thumbnail image of product.
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Active product.
        /// </summary>
        public bool IsShow { get; set; }
    }
}
