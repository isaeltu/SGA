namespace SGA.Web.Services;

public sealed class UiFeedbackService
{
    private readonly object syncRoot = new();
    private readonly List<ToastMessage> toasts = new();
    private int busyCount;

    public event Action? Changed;

    public IReadOnlyList<ToastMessage> Toasts
    {
        get
        {
            lock (syncRoot)
            {
                return toasts.ToList();
            }
        }
    }

    public bool IsBusy
    {
        get
        {
            lock (syncRoot)
            {
                return busyCount > 0;
            }
        }
    }

    public IDisposable BeginBusy()
    {
        lock (syncRoot)
        {
            busyCount++;
        }

        NotifyChanged();
        return new BusyScope(this);
    }

    public void ShowSuccess(string title, string message) => AddToast(ToastKind.Success, title, message);

    public void ShowError(string title, string message) => AddToast(ToastKind.Error, title, message);

    public void ShowInfo(string title, string message) => AddToast(ToastKind.Info, title, message);

    public void ShowWarning(string title, string message) => AddToast(ToastKind.Warning, title, message);

    public void Dismiss(Guid id)
    {
        lock (syncRoot)
        {
            var index = toasts.FindIndex(toast => toast.Id == id);
            if (index >= 0)
            {
                toasts.RemoveAt(index);
            }
        }

        NotifyChanged();
    }

    private void AddToast(ToastKind kind, string title, string message)
    {
        var toast = new ToastMessage(Guid.NewGuid(), kind, title, message, DateTimeOffset.Now);
        lock (syncRoot)
        {
            toasts.Insert(0, toast);
        }

        NotifyChanged();
        _ = AutoDismissAsync(toast.Id, TimeSpan.FromSeconds(4));
    }

    private async Task AutoDismissAsync(Guid id, TimeSpan delay)
    {
        await Task.Delay(delay).ConfigureAwait(false);
        Dismiss(id);
    }

    private void ReleaseBusy()
    {
        lock (syncRoot)
        {
            if (busyCount > 0)
            {
                busyCount--;
            }
        }

        NotifyChanged();
    }

    private void NotifyChanged() => Changed?.Invoke();

    public sealed record ToastMessage(Guid Id, ToastKind Kind, string Title, string Message, DateTimeOffset CreatedAt);

    public enum ToastKind
    {
        Info,
        Success,
        Warning,
        Error
    }

    private sealed class BusyScope : IDisposable
    {
        private UiFeedbackService? owner;

        public BusyScope(UiFeedbackService owner)
        {
            this.owner = owner;
        }

        public void Dispose()
        {
            owner?.ReleaseBusy();
            owner = null;
        }
    }
}