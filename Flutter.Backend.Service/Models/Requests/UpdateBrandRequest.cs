using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateBrandRequest : BaseBrandRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
