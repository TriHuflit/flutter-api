using Flutter.Backend.DAL.Domains;
using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseOrderRequest
    {
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
