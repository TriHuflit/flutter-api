using Flutter.Backend.DAL.Domains;

namespace Flutter.Backend.Service.Models.Requests
{
    public class ConfirmOrderRequest : BaseOrderRequest
    {
        public string Id { get; set; }

        public string VoucherId { get; set; }

        public string Description { get; set; }

        public decimal Ship { get; set; }

        public Location AddressReceive { get; set; }

        public string PhoneReceive { get; set; }

        public bool IsPayment { get; set; }
    }
}
