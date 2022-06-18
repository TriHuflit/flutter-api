using MongoDB.Bson;
using System.Collections.Generic;

namespace Flutter.Backend.DAL.Domains
{
    public class Order : AuditLogSystem
    {
        public string UserOrder { get; set; }

        public ObjectId UserId { get; set; }

        public ObjectId? VoucherId { get; set; }

        public int Ship { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public Location AddressReceive { get; set; }

        public string PhoneReceive { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public decimal TotalPrice { get; set; }

        public int Status { get; set; }

        public bool IsPayment { get; set; }

    }
}
