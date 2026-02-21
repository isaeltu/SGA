using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Enums.Trips
{
    public enum TripStatus
    {
        Scheduled = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4,
        Delayed = 5
    }
}
