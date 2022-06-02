using Flutter.Backend.DAL.Domains;
using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseOrderRequest
    {
        public List<CreateOrderDetailRequest> OrderDetails { get; set; }

    }
}
