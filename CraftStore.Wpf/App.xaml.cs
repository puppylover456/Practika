using System.Windows;
using System.Windows.Threading;

namespace CraftStore.Wpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DispatcherUnhandledException += (s, args) =>
        {
            MessageBox.Show(args.Exception.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };
    }
}

