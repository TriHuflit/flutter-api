using System;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(8), MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [MinLength(8),MaxLength(16)]
        public string Password { get; set; }

        [Required]
        public string ComfirmPassWord { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public DateTime Birth { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
    }
}
