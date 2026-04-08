using System.Windows;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SGA.WPF
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new();
        private readonly ResilienceOptions _resilienceOptions;

        public MainWindow()
        {
            InitializeComponent();
            _resilienceOptions = LoadResilienceOptions();
            _httpClient.Timeout = TimeSpan.FromSeconds(_resilienceOptions.RequestTimeoutSeconds);
            WriteOutput($"Panel de operadores listo. Timeout={_resilienceOptions.RequestTimeoutSeconds}s, Retries={_resilienceOptions.MaxRetries}.");
        }

        private async Task<int> PostForIdAsync<T>(string path, T payload, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync(path, payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidOperationException($"API {(int)response.StatusCode}: {body}");
            }

            var id = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
            return id;
        }

        private void SetBaseAddress()
        {
            var value = ApiUrlTextBox.Text.Trim();
            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException("La URL base de API no es valida.");
            }

            _httpClient.BaseAddress = uri;
        }

        private void WriteOutput(string message)
        {
            OutputTextBox.Text = $"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}{OutputTextBox.Text}";
        }

        private static int ParseInt(string value) => int.TryParse(value, out var number) ? number : 0;

        private async Task RunSafeAsync(string operationName, Func<CancellationToken, Task> action)
        {
            try
            {
                SetBaseAddress();
                await ExecuteWithRetryAsync(operationName, action);
            }
            catch (Exception ex)
            {
                WriteOutput($"{operationName}: {ex.Message}");
            }
        }

        private async Task ExecuteWithRetryAsync(string operationName, Func<CancellationToken, Task> action)
        {
            Exception? lastException = null;

            for (var attempt = 0; attempt <= _resilienceOptions.MaxRetries; attempt++)
            {
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_resilienceOptions.RequestTimeoutSeconds));
                    await action(cts.Token);
                    return;
                }
                catch (TaskCanceledException ex)
                {
                    lastException = ex;

                    if (attempt == _resilienceOptions.MaxRetries)
                    {
                        break;
                    }

                    var delay = TimeSpan.FromMilliseconds(_resilienceOptions.InitialBackoffMs * Math.Pow(2, attempt));
                    WriteOutput($"{operationName}: timeout, reintentando ({attempt + 1}/{_resilienceOptions.MaxRetries})...");
                    await Task.Delay(delay);
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;

                    if (attempt == _resilienceOptions.MaxRetries)
                    {
                        break;
                    }

                    var delay = TimeSpan.FromMilliseconds(_resilienceOptions.InitialBackoffMs * Math.Pow(2, attempt));
                    WriteOutput($"{operationName}: error de red, reintentando ({attempt + 1}/{_resilienceOptions.MaxRetries})...");
                    await Task.Delay(delay);
                }
            }

            if (lastException is TaskCanceledException)
            {
                throw new InvalidOperationException($"{operationName}: timeout al contactar la API tras reintentos.");
            }

            if (lastException is HttpRequestException httpEx)
            {
                throw new InvalidOperationException($"{operationName}: error de red/API tras reintentos -> {httpEx.Message}");
            }
        }

        private static ResilienceOptions LoadResilienceOptions()
        {
            var options = new ResilienceOptions();

            try
            {
                var configPath = System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                if (System.IO.File.Exists(configPath))
                {
                    var json = System.IO.File.ReadAllText(configPath);
                    using var document = JsonDocument.Parse(json);
                    if (document.RootElement.TryGetProperty("Resilience", out var section))
                    {
                        if (section.TryGetProperty("RequestTimeoutSeconds", out var timeoutElement) && timeoutElement.TryGetInt32(out var timeout))
                        {
                            options.RequestTimeoutSeconds = timeout;
                        }

                        if (section.TryGetProperty("MaxRetries", out var retriesElement) && retriesElement.TryGetInt32(out var retries))
                        {
                            options.MaxRetries = retries;
                        }

                        if (section.TryGetProperty("InitialBackoffMs", out var backoffElement) && backoffElement.TryGetInt32(out var backoff))
                        {
                            options.InitialBackoffMs = backoff;
                        }
                    }
                }
            }
            catch
            {
                // Keep defaults when config is missing or malformed.
            }

            if (options.RequestTimeoutSeconds <= 0)
            {
                options.RequestTimeoutSeconds = 20;
            }

            if (options.MaxRetries < 0)
            {
                options.MaxRetries = 2;
            }

            if (options.InitialBackoffMs <= 0)
            {
                options.InitialBackoffMs = 400;
            }

            return options;
        }

        private async void CreateInstitution_Click(object sender, RoutedEventArgs e)
        {
            await RunSafeAsync("Crear institucion", async ct =>
            {
                var id = await PostForIdAsync("api/institutions", new
                {
                    Code = InstitutionCodeTextBox.Text.Trim(),
                    Name = InstitutionNameTextBox.Text.Trim(),
                    CreatedBy = InstitutionCreatedByTextBox.Text.Trim()
                }, ct);

                InstitutionSearchIdTextBox.Text = id.ToString();
                WriteOutput($"Institucion creada con ID {id}.");
            });
        }

        private async void GetInstitution_Click(object sender, RoutedEventArgs e)
        {
            await RunSafeAsync("Consultar institucion", async ct =>
            {
                var id = ParseInt(InstitutionSearchIdTextBox.Text);
                var institution = await _httpClient.GetFromJsonAsync<InstitutionDto>($"api/institutions/{id}", ct);
                if (institution is null)
                {
                    WriteOutput("Institucion no encontrada.");
                    return;
                }

                WriteOutput($"Institucion {institution.Id}: {institution.Code} - {institution.Name}");
            });
        }

        private async void CreateBus_Click(object sender, RoutedEventArgs e)
        {
            await RunSafeAsync("Crear autobus", async ct =>
            {
                var id = await PostForIdAsync("api/buses", new
                {
                    InstitutionId = ParseInt(BusInstitutionIdTextBox.Text),
                    LicensePlate = BusPlateTextBox.Text.Trim(),
                    Model = BusModelTextBox.Text.Trim(),
                    Year = ParseInt(BusYearTextBox.Text),
                    Capacity = ParseInt(BusCapacityTextBox.Text),
                    CreatedBy = BusCreatedByTextBox.Text.Trim()
                }, ct);

                WriteOutput($"Autobus creado con ID {id}.");
            });
        }

        private async void LoadBuses_Click(object sender, RoutedEventArgs e)
        {
            await RunSafeAsync("Consultar autobuses", async ct =>
            {
                var buses = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<BusDto>>("api/buses", ct);
                WriteOutput($"Total autobuses: {buses?.Count ?? 0}.");
            });
        }

        private async void CreateTrip_Click(object sender, RoutedEventArgs e)
        {
            await RunSafeAsync("Crear viaje", async ct =>
            {
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
                }, ct);

                TripIdTextBox.Text = id.ToString();
                WriteOutput($"Viaje creado con ID {id}.");
            });
        }

        private async void GetTrip_Click(object sender, RoutedEventArgs e)
        {
            await GetTripInternalAsync();
        }

        private async Task GetTripInternalAsync()
        {
            await RunSafeAsync("Consultar viaje", async ct =>
            {
                var id = ParseInt(TripIdTextBox.Text);
                var trip = await _httpClient.GetFromJsonAsync<TripDto>($"api/trips/{id}", ct);
                if (trip is null)
                {
                    WriteOutput("Viaje no encontrado.");
                    return;
                }

                WriteOutput($"Viaje {trip.Id} estado {trip.Status}, asientos {trip.AvailableSeats}.");
            });
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
            await RunSafeAsync(okMessage, async ct =>
            {
                var response = await _httpClient.PostAsJsonAsync(path, new
                {
                    TripId = ParseInt(TripIdTextBox.Text),
                    ModifiedBy = TripModifiedByTextBox.Text.Trim()
                }, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    WriteOutput($"API {(int)response.StatusCode}: {body}");
                    return;
                }

                WriteOutput(okMessage);
                await GetTripInternalAsync();
            });
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
            await RunSafeAsync("Crear reserva", async ct =>
            {
                var id = await PostForIdAsync("api/reservations", new
                {
                    TripId = ParseInt(ReservationTripIdTextBox.Text),
                    PersonId = ParseInt(ReservationPersonIdTextBox.Text),
                    AuthorizationId = ParseInt(ReservationAuthorizationIdTextBox.Text),
                    CreatedBy = ReservationCreatedByTextBox.Text.Trim()
                }, ct);

                ReservationIdTextBox.Text = id.ToString();
                WriteOutput($"Reserva creada con ID {id}.");

                if (generateTicket)
                {
                    var reservation = await _httpClient.GetFromJsonAsync<ReservationDto>($"api/reservations/{id}", ct);
                    if (reservation is not null)
                    {
                        WriteOutput($"Ticket -> Reserva {reservation.Id}, asiento {ReservationSeatTextBox.Text.Trim()}, cola {reservation.QueueNumber}.");
                    }
                }
            });
        }

        private async void GetReservation_Click(object sender, RoutedEventArgs e)
        {
            await RunSafeAsync("Consultar reserva", async ct =>
            {
                var id = ParseInt(ReservationIdTextBox.Text);
                var reservation = await _httpClient.GetFromJsonAsync<ReservationDto>($"api/reservations/{id}", ct);
                if (reservation is null)
                {
                    WriteOutput("Reserva no encontrada.");
                    return;
                }

                WriteOutput($"Reserva {reservation.Id} estado {reservation.Status}, cola {reservation.QueueNumber}.");
            });
        }

        private async void BoardReservation_Click(object sender, RoutedEventArgs e)
        {
            await RunSafeAsync("Abordar reserva", async ct =>
            {
                var response = await _httpClient.PostAsJsonAsync("api/reservations/board", new
                {
                    ReservationId = ParseInt(ReservationIdTextBox.Text),
                    ModifiedBy = ReservationModifiedByTextBox.Text.Trim()
                }, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(ct);
                    WriteOutput($"API {(int)response.StatusCode}: {body}");
                    return;
                }

                WriteOutput("Reserva abordada correctamente.");
            });
        }

        private sealed record InstitutionDto(int Id, string Code, string Name, DateTime CreatedAt);
        private sealed record BusDto(int Id, int InstitutionId, string LicensePlate, string Model, int Year, int Capacity, int AvailableSeats, string Status);
        private sealed record TripDto(int Id, int InstitutionId, int RouteId, int DriverId, int BusId, string Status, DateTime ScheduledDepartureTime, DateTime ScheduledArrivalTime, DateTime? ActualDepartureTime, DateTime? ActualArrivalTime, int AvailableSeats);
        private sealed record ReservationDto(int Id, int TripId, int PersonId, int AuthorizationId, int QueueNumber, string QrCode, string Status, DateTime CreatedAt);

        private sealed class ResilienceOptions
        {
            public int RequestTimeoutSeconds { get; set; } = 20;
            public int MaxRetries { get; set; } = 2;
            public int InitialBackoffMs { get; set; } = 400;
        }
    }
}