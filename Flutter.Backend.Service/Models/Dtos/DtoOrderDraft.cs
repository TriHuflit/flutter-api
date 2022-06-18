using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoOrderDraft
    {
        public string Id { get; set; }

        public IEnumerable<DtoOrderDetail> OrderDetails { get; set; }
    }
}
