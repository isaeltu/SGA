using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.Enums.Incidents
{
    public enum IncidentType
    {
        Delay = 1,
        MechanicalFailure = 2,
        Accident = 3,
        RouteDeviation = 4,
        Overcrowding = 5,
        SecurityIssue = 6,
        Other = 7
    }
}
