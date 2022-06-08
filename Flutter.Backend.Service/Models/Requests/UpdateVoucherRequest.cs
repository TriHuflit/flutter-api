using System;
using System.ComponentModel.DataAnnotations;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateVoucherRequest
    {
        public string Id { get; set; }


        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public string Description { get; set; }

        public int IsShow { get; set; }
    }
}
