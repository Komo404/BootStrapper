using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateCreateViewModel : ViewModelBase
{
    public string TemplateCreateMainTEXT { get; set; } = "Hello from TEMPLATE CREATE";
    private readonly NavigationService _navigation;

    public TemplateCreateViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation);
    }

    [RelayCommand]
    private void GoToTemplateInfo()
    {
        _navigation.CurrentView = new TemplateInfoViewModel(_navigation);
    }
}
