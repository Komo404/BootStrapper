using BootStrapper.Core.Models;
using BootStrapper.Helpers;
using BootStrapper.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper;

public partial class NavigationService : ObservableObject
{

    [ObservableProperty] private ViewModelBase? _currentView;

    public IOpenExplorer Explorer { get; }
    public UserConfig Config { get; set; }

    public NavigationService(IOpenExplorer _explorer, UserConfig _config)
    {
        Explorer = _explorer;
        Config = _config;
    }
}
