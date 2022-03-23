using System;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoAuthent
    {
        public string AccessToken { get; set; }

        public DateTime Expires { get; set; }

    }
}
