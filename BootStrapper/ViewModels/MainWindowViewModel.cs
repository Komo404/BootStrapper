using BootStrapper.ViewModels.Projects;
using BootStrapper.ViewModels.Templates;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace BootStrapper.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        public NavigationService Navigation { get; }

        public MainWindowViewModel()
        {
            Navigation = new NavigationService();

            Navigation.CurrentView = new HomeViewModel(Navigation);
        }

    }
}
