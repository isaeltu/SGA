using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGA.Domain.ValueObjects.Users
{
    public sealed class EmployeeCode
    {
        public string Value { get; private set; }

        private EmployeeCode() { Value = string.Empty; }  

        
        public EmployeeCode(string value)
        {
            Validate(value);  
            Value = value.Trim().ToUpperInvariant();
        }

        private void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Employee code cannot be empty");

            
            if (value.Length != 8)
                throw new ArgumentException("Employee code must be 8 characters");

            if (!value.StartsWith("EMP", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Employee code must start with 'EMP'");

            var numericPart = value.Substring(3);
            if (!int.TryParse(numericPart, out _))
                throw new ArgumentException("Employee code must end with 5 digits");
        }

        // Igualdad por valor...
        public bool Equals(EmployeeCode? other) =>
            other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Value.ToUpperInvariant().GetHashCode();
        public override string ToString() => Value;
    }
}
