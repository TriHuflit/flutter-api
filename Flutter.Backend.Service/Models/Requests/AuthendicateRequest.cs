using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class AuthendicateRequest
    {
        [Required]
        [MinLength(8), MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [MinLength(8),MaxLength(16)]
        public string Password { get; set; }
    }
}
