using System;

namespace SGA.Domain.ValueObjects.Transportation
{
    public sealed class Coordinate : IEquatable<Coordinate>
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        private Coordinate()
        {
        }

        public Coordinate(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("Latitude must be between -90 and 90", nameof(latitude));

            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));

            Latitude = latitude;
            Longitude = longitude;
        }

        public bool Equals(Coordinate? other)
        {
            if (other is null)
                return false;

            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override bool Equals(object? obj) => obj is Coordinate coordinate && Equals(coordinate);
        public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);
        public override string ToString() => $"{Latitude:F6}, {Longitude:F6}";

        public static bool operator ==(Coordinate? left, Coordinate? right) =>
            left?.Equals(right) ?? right is null;

        public static bool operator !=(Coordinate? left, Coordinate? right) =>
            !(left == right);
    }
}
