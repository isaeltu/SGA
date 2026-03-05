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
        public int RouteId { get; set; }
        public int StopId { get; set; }
        
        public int StopOrder { get;  set; }
        public int EstimatedArrivalMinutes { get; set; }
        
        public Route Route { get; set; } = null!;
        public Stop Stop { get; set; } = null!;
        
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
