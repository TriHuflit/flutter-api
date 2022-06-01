using Flutter.Backend.DAL.Domains;
using System.Collections.Generic;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoOrder : DtoAuditLogSystem
    {
        public int Id { get; set; }

        public string VoucherId { get; set; }

        public Ship Ship { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public string AddressReceive { get; set; }

        public string PhoneReceive { get; set; }

        public string Description { get; set; }

        public decimal TotalPrice { get; set; }

        public int Status { get; set; }

        public bool IsPayment { get; set; }
    }
}
