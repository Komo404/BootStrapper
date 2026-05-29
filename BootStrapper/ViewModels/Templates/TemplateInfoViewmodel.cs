using BootStrapper.Core.Models;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateInfoViewModel : ViewModelBase
{
    public string TemplateInfoMainTEXT { get; set; } = "Hello from TEMPLATE INFO";
    private readonly NavigationService _navigation;
    private readonly TemplateManifest _template;

    public string TemplateName => _template.Name;
    public string TemplateDesc => _template.Description;
    public string TemplateMinUnityV => _template.MinUnityVersion;
    public string TemplateMaxUnityV => _template.MaxUnityVersion;

    public TemplateInfoViewModel(NavigationService navigation, TemplateManifest template)
    {
        _navigation = navigation;
        _template = template;
    }
}
