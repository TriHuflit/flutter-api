using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateAvatarRequest
    {
        [Required]
        public string Avatar { get; set; }
    }
}
