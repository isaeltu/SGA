
using System.Diagnostics.Tracing;
using Microsoft.EntityFrameworkCore;
using SGA.Domain.Common;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Enums.Authorizations;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Repositories;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories.SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Trips
{
    public class ReservationRepository : Repository<Reservation, int>, IReservationRepository
    {
        
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<ICollection<Result<Reservation>>> GetAllReservationsById(int tripId)
        {
           var reservations = await _entities
                .Include(r => r.Authorization)
                .Where(r => r.TripId == tripId)
                .OrderBy(r => r.QueueNumber)
                .AsNoTracking()
                .ToListAsync();
            if(!reservations.Any())
            {
                Result<Reservation>.Failure(new Error("", ""));
            }

            return reservations
            .Select(r => Result<Reservation>.Success(r))
            .ToList();

        }

        public async Task<int> GetNextQueueNumberAsync(int tripId, CancellationToken cancellationToken = default)
        {
            return await 
        }

        public async Task<Result<Reservation?>> GetReservationByQrCodeAsync(string qrCode, CancellationToken cancellationToken = default)
        {
            var reservation = await _entities
                .FindAsync(qrCode);

            if(reservation == null)
            {
               return Result<Reservation?>.Failure(new Error("Reservation.NotFound",
                $"No se encontró ninguna reservación con el código QR:"));
            }

            return Result<Reservation?>.Success(reservation);
            
        }

        public async Task<IReadOnlyCollection<Reservation>> GetReservationsByStatusAsync(ReservationStatus status, CancellationToken cancellationToken = default)
        {
             var reservations = await _context.Reservations.


        }

        public Task<IReadOnlyCollection<Reservation>> GetReservationsByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Reservation>> GetReservationsByTripIdAsync(int tripId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasActiveReservationAsync(int studentId, int tripId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
