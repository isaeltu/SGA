using SGA.Domain.Base;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Entities.Users;
using System;

namespace SGA.Domain.Entities.Trips
{
    public class Boarding : BaseEntity<int>
    {
        public int ReservationId { get; protected set; }
        public int TripId { get; protected set; }
        public int StudentId { get; protected set; }
        
        public DateTime BoardingTime { get; protected set; }
        public string BoardingStop { get; protected set; } = string.Empty;
        
        public BoardingType BoardingType { get; protected set; }
        
        public Reservation Reservation { get; protected set; } = null!;
        public Trip Trip { get; protected set; } = null!;
        public Student Student { get; protected set; } = null!;
        
        protected Boarding() { }
        
        public Boarding(
            int reservationId,
            int tripId,
            int studentId,
            string boardingStop,
            BoardingType boardingType,
            string createdBy)
        {
            ReservationId = reservationId;
            TripId = tripId;
            StudentId = studentId;
            BoardingTime = DateTime.UtcNow;
            BoardingStop = boardingStop;
            BoardingType = boardingType;
            
            SetCreationInfo(createdBy);
        }
    }
}
