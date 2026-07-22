using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Helpers;
using BootStrapper.ViewModels;
using BootStrapper.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using Velopack;

namespace BootStrapper;

public partial class App : Application // Ponto inicial da aplicação
{
    internal static string ConfigFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BootStrapper", "config.json");

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        DataTemplates.Add(new ViewLocator());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Load user settings
            UserConfig config = ConfigService.LoadConfig(ConfigFilePath);


            var mainWindow = new MainWindow();
            IOpenExplorer explorer = new OpenExplorer(mainWindow);

            // Load the MainWindow with the things it needs. MainWindowViewModel Instantiates the Main Services.
            mainWindow.DataContext = new MainWindowViewModel(config, explorer);

            desktop.MainWindow = mainWindow;
            _ = CheckForUpdatesAsync(config);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async Task CheckForUpdatesAsync(UserConfig config)
    {
        if (!config.AutoUpdateEnabled) return;

        try
        {
            var updManager = new UpdateManager("https://github.com/Komo404/BootStrapper");

            var newVersion = await updManager.CheckForUpdatesAsync();
            if (newVersion == null) return;

            await updManager.DownloadUpdatesAsync(newVersion);
            updManager.ApplyUpdatesAndRestart(newVersion);
        } catch (Exception ex)
        {
            Console.WriteLine($"Error checking for updates: {ex.Message}");
        }
    }
}
