using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Requests
{
    public class CreateFeedBackRequest
    {
        public string ProductId { get; set; }

        public string Content { get; set; }

        public int Star { get; set; }

        public List<string> ImageFeedBack { get; set; }
    }
}
