using SGA.Domain.Base;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Entities.Users;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.DomainEvents.Trips;

namespace SGA.Domain.Entities.Trips
{
    public class Reservation : BaseEntity<int>, IAuditEntity
    {
        public int TripId { get;  set; }
        public int PersonId {get; set;}
        public int AuthorizationId { get;  set; }
        
        public int QueueNumber { get;  set; }
        public string QrCode { get; set; } = string.Empty;
        
        public ReservationStatus Status { get;  set; }
        
        public Trip Trip { get;  set; } = null!;
        public Person Student { get;  set; } = null!;
        public Authorization Authorization { get; set; } = null!;
        public Boarding? Boarding { get;  set; }
    
        protected Reservation() { }
        
        public Reservation(
            int tripId,
            int personId,
            int authorizationId,
            int queueNumber,
            string createdBy)
        {
            TripId = tripId;
            PersonId = personId;
            AuthorizationId = authorizationId;
            QueueNumber = queueNumber;
            QrCode = GenerateQrCode();
            Status = ReservationStatus.Confirmed;
            
            SetCreationInfo(createdBy);
            AddDomainEvent(new ReservationConfirmedDomainEvent(Id, TripId, PersonId));
        }
        
        private string GenerateQrCode()
        {
            QrCode = $"RES-{Guid.NewGuid().ToString().Substring(0,8).ToUpper()}";
            return QrCode ;
        }
        
        public void MarkAsBoarded(string modifiedBy)
        {
            if (Status != ReservationStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed reservations can be boarded");
            
            Status = ReservationStatus.Boarded;
            SetModificationInfo(modifiedBy);
            
        }
        
        public void Cancel(string modifiedBy)
        {
            if (Status == ReservationStatus.Boarded)
                throw new InvalidOperationException("Cannot cancel boarded reservations");
            
            Status = ReservationStatus.Cancelled;
            SetModificationInfo(modifiedBy);
            AddDomainEvent(new ReservationCancelledDomainEvent(Id, TripId, PersonId));
        }
    }
}
