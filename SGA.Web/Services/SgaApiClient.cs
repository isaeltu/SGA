using System.Net.Http.Json;
using SGA.Web.Models;

namespace SGA.Web.Services;

public sealed class SgaApiClient
{
    private readonly HttpClient _httpClient;

    public SgaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> CreateInstitutionAsync(CreateInstitutionRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/institutions", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<InstitutionResponse?> GetInstitutionByIdAsync(int institutionId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<InstitutionResponse>($"api/institutions/{institutionId}", cancellationToken);

    public async Task<int> CreateBusAsync(CreateBusRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/buses", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<IReadOnlyCollection<BusResponse>?> GetBusesAsync(CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<IReadOnlyCollection<BusResponse>>("api/buses", cancellationToken);

    public Task<BusResponse?> GetBusByIdAsync(int busId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<BusResponse>($"api/buses/{busId}", cancellationToken);

    public async Task<int> CreateReservationAsync(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reservations", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<ReservationResponse?> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<ReservationResponse>($"api/reservations/{reservationId}", cancellationToken);

    public async Task BoardReservationAsync(BoardReservationRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reservations/board", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateTripAsync(CreateTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<TripResponse?> GetTripByIdAsync(int tripId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<TripResponse>($"api/trips/{tripId}", cancellationToken);

    public async Task StartTripAsync(StartTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips/start", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public async Task CompleteTripAsync(CompleteTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips/complete", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public async Task CancelTripAsync(CancelTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips/cancel", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        throw new InvalidOperationException($"API error {(int)response.StatusCode}: {body}");
    }
}
