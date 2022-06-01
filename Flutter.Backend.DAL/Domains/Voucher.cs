using System;

namespace Flutter.Backend.DAL.Domains
{
    public class Voucher : AuditLogSystem
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int DisCountPercent { get; set; }

        public int DisCountAmount { get; set; }

        public int FromCondition { get; set; }

        public int ToCondition { get; set; }

        public int IsShow { get; set; }
    }
}
