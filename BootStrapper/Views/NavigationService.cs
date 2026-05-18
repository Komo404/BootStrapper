using BootStrapper.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Views
{
    public partial class NavigationService : ObservableObject
    {
        [ObservableProperty]
        private ViewModelBase _currentView;
    }
}
