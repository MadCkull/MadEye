using CommunityToolkit.WinUI.UI.Controls;
using MadEye.ViewModels;
using Microsoft.UI.Xaml;
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

    private void AdaptiveGridView_Loaded(object sender, RoutedEventArgs e)
    {
        // Get the parent container of the AdaptiveGridView
        FrameworkElement parentContainer = (sender as AdaptiveGridView).Parent as FrameworkElement;

        if (parentContainer != null)
        {
            // Calculate the available width for the AdaptiveGridView
            double availableWidth = parentContainer.ActualWidth;

            // Calculate the desired width for each item in the grid
            double desiredItemWidth = availableWidth / 3;

            // Set the DesiredWidth property to be the desired item width
            ((AdaptiveGridView)sender).DesiredWidth = desiredItemWidth;
        }
    }

#endregion

}
