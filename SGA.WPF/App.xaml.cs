using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace SGA.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Error no controlado en UI: {e.Exception.Message}", "SGA WPF", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }

        private void OnCurrentDomainUnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"Error critico controlado: {ex.Message}", "SGA WPF", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show($"Error asincrono controlado: {e.Exception.GetBaseException().Message}", "SGA WPF", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.SetObserved();
        }
    }

}
