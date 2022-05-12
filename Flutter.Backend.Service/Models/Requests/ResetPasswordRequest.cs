namespace Flutter.Backend.Service.Models.Requests
{
    public class ResetPasswordRequest
    {

        public string UserName { get; set; }

        public string NewPassword { get; set; }

        public string Email { get; set; }
    }
}
