using System;

namespace Flutter.Backend.DAL.Domains
{
    public class Voucher : AuditLogSystem
    {
        public string Title  { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int DisCountPercent { get; set; }

        public decimal DisCountAmount { get; set; }

        public decimal LimitDisCountAmout { get; set; }

        public string ImageVoucher { get; set; }

        public decimal FromCondition { get; set; }

        public decimal ToCondition { get; set; }
       
        public string Description { get; set; }

        public string Type { get; set; }

        public int IsShow { get; set; }
    }
}
