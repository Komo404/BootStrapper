using BootStrapper.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateListViewModel : ViewModelBase
{
    public string TemplateListTEXT { get; set; } = "Hello from Template List";
    private readonly NavigationService _navigation;

    public TemplateListViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    public TemplateListViewModel() { }
}
