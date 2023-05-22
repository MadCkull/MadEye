using CommunityToolkit.WinUI.UI.Controls;
using MadEye.Core.Models;
using MadEye.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI.Popups;

namespace MadEye.Views;

public sealed partial class HomePage : Page
{

    private readonly string Username = "a";
    private readonly string Password = "b";

    private readonly bool isLoggedin = MadEye.GlobalClasses.GlobalSingletonClass.Instance.isLoggedin;


    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        
        InitializeComponent();

        Btn_Login_Click(null, null); //Tmp
    }



    #region User Defined Methods:



    //Keeps Modules Alligned when Size of App is Changed
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
    
    private void Btn_Login_Click(object sender, RoutedEventArgs e)
    {
        //if (Inpt_Username.Text == Username && Inpt_Password.Password == Password)
        if(true)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            ModulesGrid.Visibility = Visibility.Visible;
            MadEye.GlobalClasses.GlobalSingletonClass.Instance.isLoggedin = true;
        }
        else
        {
            LoginError.Visibility = Visibility.Visible;
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (MadEye.GlobalClasses.GlobalSingletonClass.Instance.isLoggedin == true)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            ModulesGrid.Visibility = Visibility.Visible;
        }
        else
        {
            ModulesGrid.Visibility = Visibility.Collapsed;
        }
    }

    private void Inpt_Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
        LoginError.Visibility = Visibility.Collapsed;
    }

    private void Inpt_Username_TextChanged(object sender, TextChangedEventArgs e)
    {
        LoginError.Visibility = Visibility.Collapsed;
    }
}
