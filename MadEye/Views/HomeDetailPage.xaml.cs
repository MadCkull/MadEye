using System.Xml.Linq;
using CommunityToolkit.WinUI.UI.Animations;

using MadEye.Contracts.Services;
using MadEye.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Data.SqlTypes;
using System.Globalization;

using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Windowing;
using MadEye.GlobalClasses;
using CommunityToolkit.WinUI.UI.Controls;

namespace MadEye.Views;


public sealed partial class HomeDetailPage : Page
{
    public HomeDetailViewModel ViewModel
    {
        get;
    }
    

    public HomeDetailPage()
    {
        
        InitializeComponent();

        ViewModel = App.GetService<HomeDetailViewModel>();
        DataContext = ViewModel;
        
        ViewModel.TotalEntries = TotalEntries;

        InitilizeModule();
    }



#region > Template Code (Do NOT Modify)


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }

    #endregion



#region > User Defined:

    #region - Main Area:

    public string SelectedDate = ((ShellPage)App.MainWindow.Content).Selected_Date;
    readonly string TotalEntries = "0";


    private void InitilizeModule()
    {
        //Gets Module ID From SingletonClass (A Global Class that can be accessed from anywhere, used For passing Data between different Classes)
        var ClickedModule = MadEye.GlobalClasses.GlobalSingletonClass.Instance.SelectedHomeModuleID;

        if (ClickedModule == 10643)
        {
            KeystrokesModuleInitialize();
        }
        if(ClickedModule == 10646)
        {
            HistoryModuleInitialize();
        }
        if (ClickedModule == 10644)
        {
            ScreenshotsModuleInitialize();
        }
    }

    #endregion

    #region - Internet History Module:

    public void HistoryModuleInitialize()
    {
        BrowserHistoryModule.Visibility = Visibility.Visible;
        ViewModel.HistoryStackContainer = HistoryStackContainer;
        ViewModel.HistoryLoadButton = HistoryLoadButton;
        
        ViewModel.GetChromeHistory();
        ViewModel.SetHistory();
    }

    private void HistoryLoadButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SetHistory();
    }

    #endregion


    #region - Keystrokes Capture Module:

    public void KeystrokesModuleInitialize()
    {
        KeystrokesModule.Visibility = Visibility.Visible;
        ViewModel.KeystrokesStackContainer = KeystrokesStackContainer;
        ViewModel.KeystrokesLoadButton = KeystrokesLoadButton;

        ViewModel.GetCapturedKeystrokes();
        ViewModel.SetKeystrokes();
    }

    private void KeystrokesLoadButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SetKeystrokes();
    }

    #endregion


    #region - Screenshot Module

    public void ScreenshotsModuleInitialize()
    {
        ScreenshotsModule.Visibility = Visibility.Visible;
        ViewModel.ScreenshotsStackContainer = ScreenshotsStackContainer;
        ViewModel.ScreenshotsLoadButton = ScreenshotsLoadButton;

        ViewModel.GetScreenshots();
        ViewModel.SetScreenshots();
    }

    private void ScreenshotsLoadButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SetScreenshots();
    }

    #endregion

    #endregion

}
