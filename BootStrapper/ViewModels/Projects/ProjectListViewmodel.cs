using BootStrapper.Views;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectListViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;

    public ProjectListViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

}
