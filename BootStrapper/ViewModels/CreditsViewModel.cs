namespace BootStrapper.ViewModels;

public class CreditsViewModel : ViewModelBase
{
    private readonly NavigationService? _navigation;
    public CreditsViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }
}
