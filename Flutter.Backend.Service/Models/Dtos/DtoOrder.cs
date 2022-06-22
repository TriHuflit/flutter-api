using Flutter.Backend.DAL.Domains;
using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoOrder : DtoAuditLogSystem
    {
        public string UserOrder { get; set; }

        public string UserId { get; set; }

        public string Id { get; set; }

        public string VoucherId { get; set; }

        public decimal Ship { get; set; }

        public IEnumerable<DtoOrderDetail> OrderDetails { get; set; }

        public Location AddressReceive { get; set; }

        public string PhoneReceive { get; set; }

        public string Description { get; set; }

        public decimal TotalPrice { get; set; }

        public string Email { get; set; }

        public int Status { get; set; }

        public bool IsPayment { get; set; }
    }
}
