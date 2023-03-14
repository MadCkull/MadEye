using MadEye.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace MadEye.Views;

public sealed partial class RecWarePage : Page
{
    public RecWareViewModel ViewModel
    {
        get;
    }

    public RecWarePage()
    {
        ViewModel = App.GetService<RecWareViewModel>();
        InitializeComponent();
    }
}
