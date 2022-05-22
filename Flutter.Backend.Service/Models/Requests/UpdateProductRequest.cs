using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateProductRequest : BaseProductRequest
    {
        /// <summary>
        /// Id of product.
        /// </summary>
        [Required]
        public string Id { get; set; }


        /// <summary>
        /// Gets or sets the classify products.
        /// </summary>
        [Required]
        public List<UpdateClassifyProductRequest> ClassifyProducts { get; set; }
    }
}
