using Flutter.Backend.Service.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Responses
{
    public class SearchResultData
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<DtoProduct> ListProduct { get; set; }
    }
}
