using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateClassifyProductRequest : BaseClassifyProductRequest
    {
        public string Id { get; set; }
    }
}
