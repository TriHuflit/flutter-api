using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Domains
{
    public class MessageRes
    {
        public ObjectId Id { get; set; }

        public string MessageResponse { get; set; }

        public string Key { get; set; }
    }
}