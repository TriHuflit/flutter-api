using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateRequestProduct : BaseRequestProduct
    {
        /// <summary>
        /// Id of product.
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}
