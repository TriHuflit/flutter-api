using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Domains
{
    public class Category : AuditLogSystem
    {
        public string name { get; set; }

        public string imageCategory { get; set; }

        public int IsShow { get; set; }
    }
}
