using System;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoRefreshToken
    {
        public string AccessToken { get; set; }

        public DateTime ExpiresAccess { get; set; }
    }
}
