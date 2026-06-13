using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.ViewModels.Projects;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    public ObservableCollection<Project> RecentProjects { get; set; }

    public SettingsViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
    }
}
