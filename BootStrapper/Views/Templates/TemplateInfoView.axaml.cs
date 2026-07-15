using BootStrapper.ViewModels.Templates;
using Avalonia.Controls;

namespace BootStrapper.Views.Templates;

public partial class TemplateInfoView : UserControl
{
    public TemplateInfoView()
    {
        InitializeComponent();
        Loaded += async (_, _) =>
        {
            if (DataContext is TemplateInfoViewModel viewModel)
                await viewModel.LoadUnityVersionsCommand.ExecuteAsync(null);
        };
    }
}