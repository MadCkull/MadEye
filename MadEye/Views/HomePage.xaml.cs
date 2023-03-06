using MadEye.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI.Popups;

namespace MadEye.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
        
    }



#region User Defined Methods:
    private void WinSize()
    {
        
    }

#endregion



    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        WinSize();
    }

}
