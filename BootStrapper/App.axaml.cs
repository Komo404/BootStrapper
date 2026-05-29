using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.ViewModels;
using BootStrapper.Views;
using System;
using System.IO;
using System.Linq;

namespace BootStrapper;

public partial class App : Application
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

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(config),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}