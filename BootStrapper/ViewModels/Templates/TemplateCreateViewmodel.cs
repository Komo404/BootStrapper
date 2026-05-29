using BootStrapper.Core.Models;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;

    public TemplateCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;
    }

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void GoToTemplateInfo(TemplateManifest template)
    {
        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, template);
    }
}
