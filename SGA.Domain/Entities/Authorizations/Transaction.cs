using SGA.Domain.Base;
using SGA.Domain.Enums.Authorizations;
using SGA.Domain.ValueObjects.Authorizations;
using SGA.Domain.Entities.Trips;
using System;

namespace SGA.Domain.Entities.Authorizations
{
    public class Transaction : BaseEntity<int>
    {
        public int AuthorizationId { get; protected set; }
        public int? TripId { get; protected set; }
        
        public Money Amount { get; protected set; } = null!;
        
        public DateTime TransactionDate { get; protected set; }
        public string? Description { get; protected set; }
        
        public TransactionType Type { get; protected set; }
        
        public Authorization Authorization { get; protected set; } = null!;
        public Trip? Trip { get; protected set; }
        
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
