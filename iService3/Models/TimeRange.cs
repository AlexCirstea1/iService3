using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Models
{
    public class TimeRange
    {
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public TimeRange() { }

        public TimeRange(TimeSpan startTime)
        {
            this.startTime = startTime;
            this.endTime = startTime.Add(TimeSpan.FromHours(2.5));
        }
    }
}
