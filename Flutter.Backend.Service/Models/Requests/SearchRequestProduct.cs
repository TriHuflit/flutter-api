using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Requests
{
    public class SearchRequestProduct
    {
        /// <summary>
        /// Key search product
        /// </summary>
        public string KeySearch { get; set; }

        /// <summary>
        /// Page Index of search
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size of search
        /// </summary>
        public int PageSize { get; set; }
    }
}
