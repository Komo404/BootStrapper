using BootStrapper.Core.Models;
using BootStrapper.Helpers;

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
