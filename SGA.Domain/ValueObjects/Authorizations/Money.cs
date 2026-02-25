using System;

namespace SGA.Domain.ValueObjects.Authorizations
{
    public sealed class Money : IEquatable<Money>
    {
        public decimal Amount { get; private set; }

        public static Money Zero => new(0m);

        private Money()
        {
            Amount = 0m;
        }

        public Money(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Money amount cannot be negative", nameof(amount));

            Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        }

        public bool Equals(Money? other)
        {
            if (other is null)
                return false;

            return Amount == other.Amount;
        }

        public override bool Equals(object? obj) => obj is Money money && Equals(money);
        public override int GetHashCode() => Amount.GetHashCode();
        public override string ToString() => Amount.ToString("0.00");

        public static bool operator ==(Money? left, Money? right) =>
            left?.Equals(right) ?? right is null;

        public static bool operator !=(Money? left, Money? right) =>
            !(left == right);
    }
}
