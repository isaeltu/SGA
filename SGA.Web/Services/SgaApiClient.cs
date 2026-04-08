using System.Net.Http.Json;
using SGA.Web.Models;

namespace SGA.Web.Services;

public sealed class SgaApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SgaApiClient> _logger;

    public SgaApiClient(HttpClient httpClient, ILogger<SgaApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<int> CreateInstitutionAsync(CreateInstitutionRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/institutions", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<InstitutionResponse?> GetInstitutionByIdAsync(int institutionId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<InstitutionResponse>($"api/institutions/{institutionId}", cancellationToken);

    public Task<IReadOnlyCollection<InstitutionResponse>?> GetInstitutionsAsync(CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<IReadOnlyCollection<InstitutionResponse>>("api/institutions", cancellationToken);

    public async Task<int> CreateBusAsync(CreateBusRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/buses", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<IReadOnlyCollection<BusResponse>?> GetBusesAsync(CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<IReadOnlyCollection<BusResponse>>("api/buses", cancellationToken);

    public Task<BusResponse?> GetBusByIdAsync(int busId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<BusResponse>($"api/buses/{busId}", cancellationToken);

    public async Task<int> CreateReservationAsync(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reservations", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateGuestReservationAsync(CreateGuestReservationRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reservations/guest", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<ReservationResponse?> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<ReservationResponse>($"api/reservations/{reservationId}", cancellationToken);

    public async Task BoardReservationAsync(BoardReservationRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reservations/board", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateTripAsync(CreateTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public Task<TripResponse?> GetTripByIdAsync(int tripId, CancellationToken cancellationToken)
        => _httpClient.GetFromJsonAsync<TripResponse>($"api/trips/{tripId}", cancellationToken);

    public Task<IReadOnlyCollection<TripResponse>?> GetTripsAsync(int? institutionId, bool onlyBookable, CancellationToken cancellationToken)
    {
        var url = institutionId.HasValue
            ? $"api/trips?institutionId={institutionId.Value}&onlyBookable={onlyBookable.ToString().ToLowerInvariant()}"
            : $"api/trips?onlyBookable={onlyBookable.ToString().ToLowerInvariant()}";

        return _httpClient.GetFromJsonAsync<IReadOnlyCollection<TripResponse>>(url, cancellationToken);
    }

    public async Task StartTripAsync(StartTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips/start", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public async Task CompleteTripAsync(CompleteTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips/complete", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public async Task CancelTripAsync(CancelTripRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/trips/cancel", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
    }

    public async Task<byte> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/roles", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<byte>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/permissions", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<PortalLoginResponse?> PortalLoginAsync(PortalLoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/portal-login", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<PortalLoginResponse>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<MasterOtpRequestResponse?> RequestMasterOtpAsync(MasterOtpRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/master/request-otp", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<MasterOtpRequestResponse>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<PortalLoginResponse?> VerifyMasterOtpAsync(MasterOtpVerifyRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/master/verify-otp", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<PortalLoginResponse>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreatePersonAsync(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/persons", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateStudentUserAsync(CreateStudentUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/students", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateOperatorUserAsync(CreateOperatorUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/operators", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateDriverUserAsync(CreateDriverUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/drivers", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateEmployeeUserAsync(CreateEmployeeUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/employees", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CreateAdministratorUserAsync(CreateAdministratorUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/administrators", request, cancellationToken).ConfigureAwait(false);
        await EnsureSuccessLoggedAsync(response, cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken).ConfigureAwait(false);
    }

    private async Task EnsureSuccessLoggedAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        _logger.LogError("API Error {StatusCode}: {Content}", (int)response.StatusCode, body);
        throw new InvalidOperationException($"API error {(int)response.StatusCode}: {body}");
    }
}
