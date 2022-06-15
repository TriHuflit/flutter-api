using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdatePasswordRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }


    }
}
