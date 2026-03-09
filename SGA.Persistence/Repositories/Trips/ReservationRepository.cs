using Microsoft.EntityFrameworkCore;
using SGA.Domain.Common;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Repositories;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories.Trips
{
    public class ReservationRepository : Repository<Reservation, int>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Result<Reservation>>> GetAllReservationsById(int tripId)
        {
            var reservations = await Entities
                .Where(r => r.TripId == tripId)
                .OrderBy(r => r.QueueNumber)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return reservations.Select(Result<Reservation>.Success).ToList();
        }

        public async Task<IReadOnlyCollection<Reservation>> GetReservationsByTripIdAsync(int tripId, CancellationToken cancellationToken = default)
        {
            return await Entities
                .Where(r => r.TripId == tripId)
                .OrderBy(r => r.QueueNumber)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<Reservation>> GetReservationsByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
        {
            return await Entities
                .Where(r => r.PersonId == studentId)
                .OrderByDescending(r => r.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Result<Reservation?>> GetReservationByQrCodeAsync(string qrCode, CancellationToken cancellationToken = default)
        {
            var reservation = await Entities
                .FirstOrDefaultAsync(r => r.QrCode == qrCode, cancellationToken)
                .ConfigureAwait(false);

            if (reservation is null)
            {
                return Result<Reservation?>.Failure(new Error("Reservation.NotFound", "No se encontro ninguna reservacion con ese QR."));
            }

            return Result<Reservation?>.Success(reservation);
        }

        public async Task<int> GetNextQueueNumberAsync(int tripId, CancellationToken cancellationToken = default)
        {
            var maxQueue = await Entities
                .Where(r => r.TripId == tripId)
                .Select(r => (int?)r.QueueNumber)
                .MaxAsync(cancellationToken)
                .ConfigureAwait(false);

            return (maxQueue ?? 0) + 1;
        }

        public async Task<bool> HasActiveReservationAsync(int studentId, int tripId, CancellationToken cancellationToken = default)
        {
            return await Entities
                .AnyAsync(r => r.PersonId == studentId && r.TripId == tripId && r.Status == ReservationStatus.Confirmed, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<Reservation>> GetReservationsByStatusAsync(ReservationStatus status, CancellationToken cancellationToken = default)
        {
            return await Entities
                .Where(r => r.Status == status)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
