using System;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class BaseVoucherRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public int DisCountPercent { get; set; }

        public decimal DisCountAmount { get; set; }

        public decimal LimitDisCountAmout { get; set; }

        [Required]
        public decimal FromCondition { get; set; }

        public decimal ToCondition { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }

        public int IsShow { get; set; }

    }
}
