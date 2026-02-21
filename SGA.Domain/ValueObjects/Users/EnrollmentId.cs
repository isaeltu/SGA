using System;
using System.Text.RegularExpressions;

namespace SGA.Domain.ValueObjects.Users
{
    public sealed class EnrollmentId : IEquatable<EnrollmentId>
    {
        private static readonly Regex EnrollmentRegex = new(
            @"^\d{4}-\d{4}$",
            RegexOptions.Compiled);
        
        public string Value { get; private set; }
        
        private EnrollmentId() { Value = string.Empty; }
        
        public EnrollmentId(string value)
        {
            Validate(value);
            Value = value.Trim().ToUpperInvariant();
        }
        
        private void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Enrollment ID cannot be empty");
            
            if (!EnrollmentRegex.IsMatch(value))
                throw new ArgumentException("Enrollment ID must be in format YYYY-NNNN (e.g., 2024-0001)");
        }
        
        public bool Equals(EnrollmentId? other) =>
            other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        
        public override bool Equals(object? obj) => obj is EnrollmentId id && Equals(id);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
        
        public static bool operator ==(EnrollmentId? left, EnrollmentId? right) =>
            left?.Equals(right) ?? right is null;
        
        public static bool operator !=(EnrollmentId? left, EnrollmentId? right) =>
            !(left == right);
    }
}
