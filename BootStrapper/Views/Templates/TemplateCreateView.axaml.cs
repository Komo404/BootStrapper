using BootStrapper.ViewModels.Templates;
using Avalonia.Controls;

namespace BootStrapper.Views.Templates;

public partial class TemplateCreateView : UserControl
{
    public TemplateCreateView()
    {
        InitializeComponent();
        Loaded += async (_, _) =>
        {
            if (DataContext is TemplateCreateViewModel viewModel)
                await viewModel.LoadUnityVersionsCommand.ExecuteAsync(null);
        };
    }
}