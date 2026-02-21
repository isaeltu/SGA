using System;
using System.Text.RegularExpressions;

namespace SGA.Domain.ValueObjects.Transportation
{
    public sealed class LicensePlate : IEquatable<LicensePlate>
    {
        private static readonly Regex PlateRegex = new(
            @"^[A-Z]\d{6}$",
            RegexOptions.Compiled);
        
        public string Value { get; private set; }
        
        private LicensePlate() { Value = string.Empty; }
        
        public LicensePlate(string value)
        {
            Validate(value);
            Value = value.Trim().ToUpperInvariant().Replace("-", "").Replace(" ", "");
        }
        
        private void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("License plate cannot be empty");
            
            var normalized = value.Trim().ToUpperInvariant().Replace("-", "").Replace(" ", "");
            
            if (!PlateRegex.IsMatch(normalized))
                throw new ArgumentException("License plate must be in format A123456");
        }
        
        public bool Equals(LicensePlate? other) =>
            other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        
        public override bool Equals(object? obj) => obj is LicensePlate plate && Equals(plate);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
        
        public static bool operator ==(LicensePlate? left, LicensePlate? right) =>
            left?.Equals(right) ?? right is null;
        
        public static bool operator !=(LicensePlate? left, LicensePlate? right) =>
            !(left == right);
    }
}
