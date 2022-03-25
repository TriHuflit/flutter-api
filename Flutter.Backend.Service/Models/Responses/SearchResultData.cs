using Flutter.Backend.Service.Models.Dtos;
using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Responses
{
    public class SearchResultData
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<DtoProduct> ListProduct { get; set; }
    }
}
