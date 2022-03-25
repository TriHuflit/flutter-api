using System;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoAuthent
    {
        public string AccessToken { get; set; }

        public DateTime ExpiresAccess { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresRefresh { get; set; }

    }
}
