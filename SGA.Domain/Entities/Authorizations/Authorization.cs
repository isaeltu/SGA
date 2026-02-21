using SGA.Domain.Base;
using SGA.Domain.Enums.Authorizations;
using SGA.Domain.ValueObjects.Authorizations;
using SGA.Domain.Entities.Users;
using SGA.Domain.Entities.Trips;
using System;
using System.Collections.Generic;

namespace SGA.Domain.Entities.Authorizations
{
    public class Authorization : BaseEntity<int>
    {
        public int StudentId { get; protected set; }
        
        public Money Balance { get; protected set; } = null!;
        public DateRange? ValidityPeriod { get; protected set; }
        
        public AuthorizationType Type { get; protected set; }
        public AuthorizationStatus Status { get; protected set; }
        
        public Student Student { get; protected set; } = null!;
        public ICollection<Transaction> Transactions { get; protected set; } = new List<Transaction>();
        public ICollection<Reservation> Reservations { get; protected set; } = new List<Reservation>();
        
        protected Authorization() { }
        
        public Authorization(
            int studentId,
            decimal initialBalance,
            string createdBy)
        {
            StudentId = studentId;
            Type = AuthorizationType.Balance;
            Balance = new Money(initialBalance);
            Status = AuthorizationStatus.Active;
            
            SetCreationInfo(createdBy);
        }
        
        public Authorization(
            int studentId,
            DateTime startDate,
            DateTime endDate,
            string createdBy)
        {
            StudentId = studentId;
            Type = AuthorizationType.Period;
            ValidityPeriod = new DateRange(startDate, endDate);
            Balance = Money.Zero;
            Status = AuthorizationStatus.Active;
            
            SetCreationInfo(createdBy);
        }
        
        public void DeductBalance(decimal amount, string modifiedBy)
        {
            if (Type != AuthorizationType.Balance)
                throw new InvalidAuthorizationException("Cannot deduct balance from non-balance authorization");
            
            if (Balance.Amount < amount)
                throw new InvalidAuthorizationException("Insufficient balance");
            
            Balance = new Money(Balance.Amount - amount);
            SetModificationInfo(modifiedBy);
        }
        
        public void AddBalance(decimal amount, string modifiedBy)
        {
            if (Type != AuthorizationType.Balance)
                throw new InvalidAuthorizationException("Cannot add balance to non-balance authorization");
            
            Balance = new Money(Balance.Amount + amount);
            SetModificationInfo(modifiedBy);
        }
        
        public bool IsValid()
        {
            if (Status != AuthorizationStatus.Active)
                return false;
            
            if (Type == AuthorizationType.Balance)
                return Balance.Amount > 0;
            
            if (Type == AuthorizationType.Period && ValidityPeriod != null)
                return ValidityPeriod.IsCurrentlyValid();
            
            return false;
        }
    }
}
