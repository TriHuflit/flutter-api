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
    }
}
