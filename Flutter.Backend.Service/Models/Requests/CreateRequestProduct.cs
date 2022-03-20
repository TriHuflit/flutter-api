using Flutter.Backend.DAL.Domains;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class CreateRequestProduct : BaseRequestProduct
    {
        /// <summary>
        /// Gets or sets the classify products.
        /// </summary>
        [Required]
        public List<CreateClassifyProductRequest> classifyProducts { get; set; }
      
    }
}
