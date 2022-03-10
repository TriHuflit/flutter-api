using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateRequestProduct : BaseRequestProduct
    {
        /// <summary>
        /// Id of product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Price at least of product.
        /// </summary>

        public decimal FromPrice { get; set; }

        /// <summary>
        /// Price at most of product.
        /// </summary>

        public decimal ToPrice { get; set; }
    }
}
