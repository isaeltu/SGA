namespace SGA.Web.Services;

public sealed class UiErrorHandler
{
    private readonly UiFeedbackService ui;
    private readonly PortalSessionService session;

    public UiErrorHandler(UiFeedbackService ui, PortalSessionService session)
    {
        this.ui = ui;
        this.session = session;
    }

    public string Handle(Exception exception, string operation)
    {
        if (exception is not ApiClientException apiError)
        {
            ui.ShowError("Operacion fallida", $"{operation}: {exception.Message}");
            return exception.Message;
        }

        var fallback = string.IsNullOrWhiteSpace(apiError.Message)
            ? "No se pudo completar la operacion."
            : apiError.Message;

        switch (apiError.StatusCode)
        {
            case 400:
            case 422:
                var detail = BuildValidationMessage(apiError.ValidationErrors) ?? fallback;
                ui.ShowWarning("Datos invalidos", detail);
                return detail;
            case 401:
                session.SignOut();
                ui.ShowWarning("Sesion expirada", "Tu sesion expiro. Inicia sesion nuevamente.");
                return "Sesion expirada";
            case 403:
                ui.ShowError("Sin permisos", "No tienes permiso para esta accion.");
                return "No tienes permiso para esta accion.";
            case 404:
                ui.ShowInfo("No encontrado", "El registro ya no existe o no esta disponible.");
                return "Recurso no encontrado.";
            case 408:
                ui.ShowWarning("Tiempo excedido", "La operacion tardo demasiado. Intenta de nuevo.");
                return "Tiempo de espera excedido.";
            case >= 500:
                var errorIdSuffix = string.IsNullOrWhiteSpace(apiError.ErrorId) ? string.Empty : $" ID: {apiError.ErrorId}";
                var serverMessage = $"Algo salio mal en el servidor.{errorIdSuffix}";
                ui.ShowError("Error del servidor", serverMessage);
                return serverMessage;
            default:
                ui.ShowError("Operacion fallida", fallback);
                return fallback;
        }
    }

    private static string? BuildValidationMessage(IDictionary<string, string[]>? errors)
    {
        if (errors is null || errors.Count == 0)
        {
            return null;
        }

        var first = errors.FirstOrDefault(kvp => kvp.Value is { Length: > 0 });
        if (string.IsNullOrWhiteSpace(first.Key))
        {
            return first.Value?.FirstOrDefault();
        }

        var fieldLabel = first.Key == "general" ? "Formulario" : first.Key;
        var detail = first.Value?.FirstOrDefault() ?? "Valor invalido.";
        return $"{fieldLabel}: {detail}";
    }
}
