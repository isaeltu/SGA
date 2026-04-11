using System.Text.Json;
using Microsoft.JSInterop;

namespace SGA.Web.Services;

public sealed class PortalSessionStorageService
{
    private const string SessionKey = "sga.portal.session";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IJSRuntime jsRuntime;

    public PortalSessionStorageService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task SaveAsync(PortalSessionService.PortalSessionSnapshot snapshot)
    {
        var payload = JsonSerializer.Serialize(snapshot, JsonOptions);
        await jsRuntime.InvokeVoidAsync("sgaUi.sessionSet", SessionKey, payload);
    }

    public async Task<PortalSessionService.PortalSessionSnapshot?> LoadAsync()
    {
        var payload = await jsRuntime.InvokeAsync<string?>("sgaUi.sessionGet", SessionKey);
        if (string.IsNullOrWhiteSpace(payload))
        {
            return null;
        }

        return JsonSerializer.Deserialize<PortalSessionService.PortalSessionSnapshot>(payload, JsonOptions);
    }

    public Task ClearAsync() => jsRuntime.InvokeVoidAsync("sgaUi.sessionRemove", SessionKey).AsTask();
}
