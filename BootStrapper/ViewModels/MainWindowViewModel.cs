using BootStrapper.ViewModels.Projects;
using BootStrapper.ViewModels.Templates;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BootStrapper.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            CurrentView = _homeView;
            Projects = "Projects";
        }

        [ObservableProperty]
        public partial string Projects { get; set; }

        [ObservableProperty]
        private ViewModelBase _currentView;

        private readonly HomeViewmodel _homeView;
        private readonly ProjectCreateViewmodel _projectCreateView;
        private readonly ProjectListViewmodel _projectListView;
        private readonly TemplateCreateViewmodel _templateCreateView;
        private readonly TemplateInfoView templateInfoView;
    }
}
