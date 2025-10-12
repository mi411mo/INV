using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Base
{
    public class Location
    {
        public string Address { get; set; }
        public string MapUrl { get; set; }
        public Position Position { get; set; }

    }
}
