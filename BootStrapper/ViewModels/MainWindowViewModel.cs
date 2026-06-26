using BootStrapper.Core.Models;
using BootStrapper.Helpers;
using BootStrapper.ViewModels.Projects;
using BootStrapper.ViewModels.Templates;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace BootStrapper.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public NavigationService Navigation { get; }

    public SidebarViewModel Sidebar { get; }

    public MainWindowViewModel(UserConfig config, IOpenExplorer explorer)
    {
        Navigation = new NavigationService(explorer, config);

        Sidebar = new SidebarViewModel(Navigation, config);

        Navigation.CurrentView = new HomeViewModel(Navigation, config);
    }

}
