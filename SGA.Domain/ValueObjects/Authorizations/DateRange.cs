using System;

namespace SGA.Domain.ValueObjects.Authorizations
{
    public sealed class DateRange : IEquatable<DateRange>
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        
        private DateRange() { }
        
        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("End date must be after start date");
            
            StartDate = startDate;
            EndDate = endDate;
        }
        
        public bool IsCurrentlyValid()
        {
            var now = DateTime.UtcNow;
            return now >= StartDate && now <= EndDate;
        }
        
        public bool Contains(DateTime date) =>
            date >= StartDate && date <= EndDate;
        
        public int DurationInDays() =>
            (EndDate - StartDate).Days;
        
        public bool Equals(DateRange? other) =>
            other is not null && StartDate == other.StartDate && EndDate == other.EndDate;
        
        public override bool Equals(object? obj) => obj is DateRange range && Equals(range);
        public override int GetHashCode() => HashCode.Combine(StartDate, EndDate);
        public override string ToString() => $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}";
        
        public static bool operator ==(DateRange? left, DateRange? right) =>
            left?.Equals(right) ?? right is null;
        
        public static bool operator !=(DateRange? left, DateRange? right) =>
            !(left == right);
    }
}
