using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class CreateProductRequest : BaseProductRequest
    {
        /// <summary>
        /// Gets or sets the classify products.
        /// </summary>
        [Required]
        public List<CreateClassifyProductRequest> ClassifyProducts { get; set; }

    }
}
