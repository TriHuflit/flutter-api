using System;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoVoucher : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int DisCountPercent { get; set; }

        public int DisCountAmount { get; set; }

        public int FromCondition { get; set; }

        public int ToCondition { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public int IsShow { get; set; }
    }
}
