using Avalonia.Controls;

namespace BootStrapper.Views;

public partial class SidebarView : UserControl
{
    public SidebarView()
    {
        InitializeComponent();

        PointerEntered += (_, _) =>
        {
            Width = 230;
            Classes.Add("expanded");
        };

        PointerExited += (_, _) =>
        {
            Width = 65;
            Classes.Remove("expanded");
        };
    }
}