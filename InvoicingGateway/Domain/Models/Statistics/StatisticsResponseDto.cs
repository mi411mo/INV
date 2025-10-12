using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Statistics
{
    public class StatisticsResponseDto
    {
        public StatisticsResponseDto(int totalRecords)
        {
            TotalRecords = totalRecords;
        }
        public int? TotalRecords { get; set; } = 0;

        
    }
}
