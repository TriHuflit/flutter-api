using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoFeedBack : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public List<string> ImageFeedBack { get; set; }

        public int Star { get; set; }
    }
}
