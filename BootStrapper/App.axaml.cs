using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Helpers;
using BootStrapper.ViewModels;
using BootStrapper.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Velopack;
using Velopack.Exceptions;

namespace BootStrapper;

public partial class App : Application // Ponto inicial da aplicação
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        DataTemplates.Add(new ViewLocator());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Carrega configurações do usuário
            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BootStrapper", "config.json");
            UserConfig config = ConfigService.LoadConfig(configFilePath);

            var mainWindow = new MainWindow();
            IOpenExplorer explorer = new OpenExplorer(mainWindow);

            // Carrega a MainWindow com as coisas que ela precisa. A Mainwindow que instancia o NavigateService. 
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
        }
        catch (NotInstalledException) { } // Não importa em dev
    }
}