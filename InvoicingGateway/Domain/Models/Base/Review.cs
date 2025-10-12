using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Base
{
    public class Review
    {
        public string CustomerName { get; set; } 
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
        public DateTime ReviewDate { get; set; }

    }
}
