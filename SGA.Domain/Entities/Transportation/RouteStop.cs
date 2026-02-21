using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGA.Domain.Base;

namespace SGA.Domain.Entities.Transportation
{
    public class RouteStop : BaseEntity<int>
    {
        public int RouteId { get; protected set; }
        public int StopId { get; protected set; }
        
        public int StopOrder { get; protected set; }
        public int EstimatedArrivalMinutes { get; protected set; }
        
        public Route Route { get; protected set; } = null!;
        public Stop Stop { get; protected set; } = null!;
        
        protected RouteStop() { }
        
        public RouteStop(
            int routeId,
            int stopId,
            int stopOrder,
            int estimatedArrivalMinutes,
            string createdBy)
        {
            RouteId = routeId;
            StopId = stopId;
            StopOrder = stopOrder;
            EstimatedArrivalMinutes = estimatedArrivalMinutes;
            
            SetCreationInfo(createdBy);
        }
    }
}
