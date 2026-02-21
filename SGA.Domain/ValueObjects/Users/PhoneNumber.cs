using System;
using System.Text.RegularExpressions;

namespace SGA.Domain.ValueObjects.Users
{
    public sealed class PhoneNumber : IEquatable<PhoneNumber>
    {
        private static readonly Regex PhoneRegex = new(
            @"^\d{10}$",
            RegexOptions.Compiled);
        
        public string Value { get; private set; }
        
        private PhoneNumber() { Value = string.Empty; }
        
        public PhoneNumber(string value)
        {
            Validate(value);
            Value = NormalizePhoneNumber(value);
        }
        
        private void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be empty");
            
            var normalized = NormalizePhoneNumber(value);
            
            if (!PhoneRegex.IsMatch(normalized))
                throw new ArgumentException("Phone number must be 10 digits");
        }
        
        private string NormalizePhoneNumber(string value)
        {
            return Regex.Replace(value, @"[^\d]", "");
        }
        
        public bool Equals(PhoneNumber? other) =>
            other is not null && Value.Equals(other.Value);
        
        public override bool Equals(object? obj) => obj is PhoneNumber phone && Equals(phone);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
        
        public static bool operator ==(PhoneNumber? left, PhoneNumber? right) =>
            left?.Equals(right) ?? right is null;
        
        public static bool operator !=(PhoneNumber? left, PhoneNumber? right) =>
            !(left == right);
    }
}
