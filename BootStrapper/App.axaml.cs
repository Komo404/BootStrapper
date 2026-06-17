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

        }

        base.OnFrameworkInitializationCompleted();
    }
}