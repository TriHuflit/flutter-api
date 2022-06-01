using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Domains
{
    public class Ship
    {
        public int From { get; set; }

        public int To { get; set; }

        public decimal Price { get; set; }
    }
}
