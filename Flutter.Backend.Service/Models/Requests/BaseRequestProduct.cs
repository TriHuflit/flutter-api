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
        public string CategoryID { get; set; }

        /// <summary>
        /// Brand of product.
        /// </summary>

        public string BrandID { get; set; }

        /// <summary>
        /// Name of product | Rules : MaxLenght 50
        /// </summary>      
        public string Name { get; set; }

        /// <summary>
        /// Short  Description of product. | Rules : MaxLenght 50
        /// </summary>

        public string Description { get; set; }

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
