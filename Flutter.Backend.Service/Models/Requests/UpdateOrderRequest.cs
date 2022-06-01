using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateOrderRequest : BaseOrderRequest
    {
        public string Id { get; set; }
    }
}
