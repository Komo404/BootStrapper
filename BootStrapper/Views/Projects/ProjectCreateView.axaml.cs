using BootStrapper.ViewModels.Projects;
using Avalonia.Controls;

namespace BootStrapper.Views.Projects;

public partial class ProjectCreateView : UserControl
{
    public ProjectCreateView()
    {
        InitializeComponent();
        Loaded += async (_, _) =>
        {
            if (DataContext is ProjectCreateViewModel viewModel)
                await viewModel.LoadUnityVersionsCommand.ExecuteAsync(null);
        };
    }
}