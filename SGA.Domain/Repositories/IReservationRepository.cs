using SGA.Domain.Common;
using SGA.Domain.Repositories.Base;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Enums.Trips;

namespace SGA.Domain.Repositories
{
    public interface IReservationRepository: IRepository<Reservation, int>
    {
        Task<ICollection<Result<Reservation>>> GetAllReservationsById(int tripId);
        Task<IReadOnlyCollection<Reservation>> GetReservationsByTripIdAsync(
            int tripId,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Reservation>> GetReservationsByStudentIdAsync(
            int studentId,
            CancellationToken cancellationToken = default);
        Task<Result<Reservation?>> GetReservationByQrCodeAsync(
            string qrCode,
            CancellationToken cancellationToken = default);

        Task<int> GetNextQueueNumberAsync(
            int tripId,
            CancellationToken cancellationToken = default);

        Task<bool> HasActiveReservationAsync(
            int studentId,
            int tripId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Reservation>> GetReservationsByStatusAsync(
            ReservationStatus status,
            CancellationToken cancellationToken = default);

    }
}
