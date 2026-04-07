using System.Windows;
using System.Net.Http;
using System.Net.Http.Json;

namespace SGA.WPF
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new();

        public MainWindow()
        {
            InitializeComponent();
            WriteOutput("Panel de operadores listo.");
        }

        private async Task<int> PostForIdAsync<T>(string path, T payload)
        {
            var response = await _httpClient.PostAsJsonAsync(path, payload);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"API {(int)response.StatusCode}: {body}");
            }

            return await response.Content.ReadFromJsonAsync<int>();
        }

        private void SetBaseAddress()
        {
            _httpClient.BaseAddress = new Uri(ApiUrlTextBox.Text.Trim());
        }

        private void WriteOutput(string message)
        {
            OutputTextBox.Text = $"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}{OutputTextBox.Text}";
        }

        private static int ParseInt(string value) => int.TryParse(value, out var number) ? number : 0;

        private async void CreateInstitution_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetBaseAddress();
                var id = await PostForIdAsync("api/institutions", new
                {
                    Code = InstitutionCodeTextBox.Text.Trim(),
                    Name = InstitutionNameTextBox.Text.Trim(),
                    CreatedBy = InstitutionCreatedByTextBox.Text.Trim()
                });

                InstitutionSearchIdTextBox.Text = id.ToString();
                WriteOutput($"Institucion creada con ID {id}.");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void GetInstitution_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetBaseAddress();
                var id = ParseInt(InstitutionSearchIdTextBox.Text);
                var institution = await _httpClient.GetFromJsonAsync<InstitutionDto>($"api/institutions/{id}");
                if (institution is null)
                {
                    WriteOutput("Institucion no encontrada.");
                    return;
                }

                WriteOutput($"Institucion {institution.Id}: {institution.Code} - {institution.Name}");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void CreateBus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetBaseAddress();
                var id = await PostForIdAsync("api/buses", new
                {
                    InstitutionId = ParseInt(BusInstitutionIdTextBox.Text),
                    LicensePlate = BusPlateTextBox.Text.Trim(),
                    Model = BusModelTextBox.Text.Trim(),
                    Year = ParseInt(BusYearTextBox.Text),
                    Capacity = ParseInt(BusCapacityTextBox.Text),
                    CreatedBy = BusCreatedByTextBox.Text.Trim()
                });

                WriteOutput($"Autobus creado con ID {id}.");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void LoadBuses_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetBaseAddress();
                var buses = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<BusDto>>("api/buses");
                WriteOutput($"Total autobuses: {buses?.Count ?? 0}.");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void CreateTrip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetBaseAddress();
                var now = DateTime.UtcNow;
                var id = await PostForIdAsync("api/trips", new
                {
                    RouteId = ParseInt(TripRouteIdTextBox.Text),
                    DriverId = ParseInt(TripDriverIdTextBox.Text),
                    BusId = ParseInt(TripBusIdTextBox.Text),
                    InstitutionId = ParseInt(TripInstitutionIdTextBox.Text),
                    ScheduledDepartureTime = now.AddMinutes(30),
                    ScheduledArrivalTime = now.AddHours(1),
                    AvailableSeats = ParseInt(TripSeatsTextBox.Text),
                    CreatedBy = TripCreatedByTextBox.Text.Trim()
                });

                TripIdTextBox.Text = id.ToString();
                WriteOutput($"Viaje creado con ID {id}.");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void GetTrip_Click(object sender, RoutedEventArgs e)
        {
            await GetTripInternalAsync();
        }

        private async Task GetTripInternalAsync()
        {
            try
            {
                SetBaseAddress();
                var id = ParseInt(TripIdTextBox.Text);
                var trip = await _httpClient.GetFromJsonAsync<TripDto>($"api/trips/{id}");
                if (trip is null)
                {
                    WriteOutput("Viaje no encontrado.");
                    return;
                }

                WriteOutput($"Viaje {trip.Id} estado {trip.Status}, asientos {trip.AvailableSeats}.");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void StartTrip_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteTripActionAsync("api/trips/start", "Viaje iniciado.");
        }

        private async void CompleteTrip_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteTripActionAsync("api/trips/complete", "Viaje completado.");
        }

        private async void CancelTrip_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteTripActionAsync("api/trips/cancel", "Viaje cancelado.");
        }

        private async Task ExecuteTripActionAsync(string path, string okMessage)
        {
            try
            {
                SetBaseAddress();
                var response = await _httpClient.PostAsJsonAsync(path, new
                {
                    TripId = ParseInt(TripIdTextBox.Text),
                    ModifiedBy = TripModifiedByTextBox.Text.Trim()
                });

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    WriteOutput($"API {(int)response.StatusCode}: {body}");
                    return;
                }

                WriteOutput(okMessage);
                await GetTripInternalAsync();
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void CreateReservationQuick_Click(object sender, RoutedEventArgs e)
        {
            await CreateReservationInternalAsync(false);
        }

        private async void CreateReservationGuided_Click(object sender, RoutedEventArgs e)
        {
            await CreateReservationInternalAsync(true);
        }

        private async Task CreateReservationInternalAsync(bool generateTicket)
        {
            try
            {
                SetBaseAddress();
                var id = await PostForIdAsync("api/reservations", new
                {
                    TripId = ParseInt(ReservationTripIdTextBox.Text),
                    PersonId = ParseInt(ReservationPersonIdTextBox.Text),
                    AuthorizationId = ParseInt(ReservationAuthorizationIdTextBox.Text),
                    CreatedBy = ReservationCreatedByTextBox.Text.Trim()
                });

                ReservationIdTextBox.Text = id.ToString();
                WriteOutput($"Reserva creada con ID {id}.");

                if (generateTicket)
                {
                    var reservation = await _httpClient.GetFromJsonAsync<ReservationDto>($"api/reservations/{id}");
                    if (reservation is not null)
                    {
                        WriteOutput($"Ticket -> Reserva {reservation.Id}, asiento {ReservationSeatTextBox.Text.Trim()}, cola {reservation.QueueNumber}.");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void GetReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetBaseAddress();
                var id = ParseInt(ReservationIdTextBox.Text);
                var reservation = await _httpClient.GetFromJsonAsync<ReservationDto>($"api/reservations/{id}");
                if (reservation is null)
                {
                    WriteOutput("Reserva no encontrada.");
                    return;
                }

                WriteOutput($"Reserva {reservation.Id} estado {reservation.Status}, cola {reservation.QueueNumber}.");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private async void BoardReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetBaseAddress();
                var response = await _httpClient.PostAsJsonAsync("api/reservations/board", new
                {
                    ReservationId = ParseInt(ReservationIdTextBox.Text),
                    ModifiedBy = ReservationModifiedByTextBox.Text.Trim()
                });

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    WriteOutput($"API {(int)response.StatusCode}: {body}");
                    return;
                }

                WriteOutput("Reserva abordada correctamente.");
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message);
            }
        }

        private sealed record InstitutionDto(int Id, string Code, string Name, DateTime CreatedAt);
        private sealed record BusDto(int Id, int InstitutionId, string LicensePlate, string Model, int Year, int Capacity, int AvailableSeats, string Status);
        private sealed record TripDto(int Id, int InstitutionId, int RouteId, int DriverId, int BusId, string Status, DateTime ScheduledDepartureTime, DateTime ScheduledArrivalTime, DateTime? ActualDepartureTime, DateTime? ActualArrivalTime, int AvailableSeats);
        private sealed record ReservationDto(int Id, int TripId, int PersonId, int AuthorizationId, int QueueNumber, string QrCode, string Status, DateTime CreatedAt);
    }
}