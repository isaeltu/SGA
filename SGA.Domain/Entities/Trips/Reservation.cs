using SGA.Domain.Base;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Entities.Users;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.DomainEvents.Trips;
using System;

namespace SGA.Domain.Entities.Trips
{
    public class Reservation : BaseEntity<int>
    {
        public int TripId { get; protected set; }
        public int StudentId { get; protected set; }
        public int AuthorizationId { get; protected set; }
        
        public int QueueNumber { get; protected set; }
        public string QrCode { get; protected set; } = string.Empty;
        
        public ReservationStatus Status { get; protected set; }
        
        public Trip Trip { get; protected set; } = null!;
        public Student Student { get; protected set; } = null!;
        public Authorization Authorization { get; protected set; } = null!;
        public Boarding? Boarding { get; protected set; }
        
        protected Reservation() { }
        
        public Reservation(
            int tripId,
            int studentId,
            int authorizationId,
            int queueNumber,
            string createdBy)
        {
            TripId = tripId;
            StudentId = studentId;
            AuthorizationId = authorizationId;
            QueueNumber = queueNumber;
            QrCode = GenerateQrCode();
            Status = ReservationStatus.Confirmed;
            
            SetCreationInfo(createdBy);
            AddDomainEvent(new ReservationConfirmedDomainEvent(Id, TripId, StudentId));
        }
        
        private string GenerateQrCode()
        {
            return $"RES-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
        
        public void MarkAsBoarded(string modifiedBy)
        {
            if (Status != ReservationStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed reservations can be boarded");
            
            Status = ReservationStatus.Boarded;
            SetModificationInfo(modifiedBy);
            AddDomainEvent(new ReservationBoardedDomainEvent(Id, TripId, StudentId));
        }
        
        public void Cancel(string modifiedBy)
        {
            if (Status == ReservationStatus.Boarded)
                throw new InvalidOperationException("Cannot cancel boarded reservations");
            
            Status = ReservationStatus.Cancelled;
            SetModificationInfo(modifiedBy);
            AddDomainEvent(new ReservationCancelledDomainEvent(Id, TripId, StudentId));
        }
    }
}
