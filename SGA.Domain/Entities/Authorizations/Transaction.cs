using SGA.Domain.Base;
using SGA.Domain.Enums.Authorizations;
using SGA.Domain.ValueObjects.Authorizations;
using SGA.Domain.Entities.Trips;
using System;

namespace SGA.Domain.Entities.Authorizations
{
    public class Transaction : BaseEntity<int>
    {
        public int AuthorizationId { get;  set; }
        public int? TripId { get; set; }
        
        public Money Amount { get;  set; } = null!;
        public DateTime TransactionDate { get;  set; }
        public string? Description { get;  set; }     
        public TransactionType Type { get;  set; }
        
        public Authorization Authorization { get;  set; } = null!;
        public Trip? Trip { get;  set; }
        
        protected Transaction() { }
        
        public Transaction(
            int authorizationId,
            decimal amount,
            TransactionType type,
            int? tripId,
            string? description,
            string createdBy)
        {
            AuthorizationId = authorizationId;
            Amount = new Money(amount);
            Type = type;
            TripId = tripId;
            Description = description;
            TransactionDate = DateTime.UtcNow;
            
            SetCreationInfo(createdBy);
        }
    }
}
