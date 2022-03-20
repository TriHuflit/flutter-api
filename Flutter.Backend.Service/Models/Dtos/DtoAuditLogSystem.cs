using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoAuditLogSystem
    {
        public string CreatedByName { get; set; }

        public DateTime CreatedByTime { get; set; }

        public string CreatedByID { get; set; }

        public string UpdatedByName { get; set; }

        public DateTime UpdatedByTime { get; set; }

        public string UpdatedByID { get; set; }
    }
}
