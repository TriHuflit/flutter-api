using System;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoRefreshToken
    {
        public string AccessToken { get; set; }

        public DateTime ExpiresAccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresRefreshToken { get; set; }
    }
}
