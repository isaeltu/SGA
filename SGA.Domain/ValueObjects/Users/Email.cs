using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.Collections;
using Thinktecture.IdentityModel.Tokens;
namespace SGA.Domain.ValueObjects.Users

{
    public sealed class Email : IEquatable<Email>
    {
        

        public string Value { get; private set; }


        private Email()
        {
            Value = string.Empty;
        }

        public Email(string value)
        {
            ValidateEmail(value);
            Value = value.Trim().ToLowerInvariant();
        }

        private void ValidateEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty", nameof(value));

            if (value.Length > 100)
                throw new ArgumentException("Email cannot exceed 100 characters", nameof(value));

            if (!EmailRegex.IsMatch(value))
                throw new ArgumentException($"Email format '{value}' is invalid", nameof(value));
        }
        public bool Equals(Email? other)
        {
            if (other is null) return false;
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj) => obj is Email email && Equals(email);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

        public override string ToString() => Value;

       
        public static bool operator ==(Email? left, Email? right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Email? left, Email? right) => !(left == right);

        
        public static implicit operator string(Email email) => email.Value;

        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(1));
    }
}

