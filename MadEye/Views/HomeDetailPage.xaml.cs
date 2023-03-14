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

        // Internet History Module:
        ViewModel.HistoryStackContainer = HistoryStackContainer;
        ViewModel.HistoryLoadButton = HistoryLoadButton;
        ViewModel.GetChromeHistory();
        ViewModel.FetchHistory();
    }

    #region Template Code (Do NOT Modify)


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

#region User Defined:


    #region Internet History Module

    



    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.FetchHistory();
    }
    #endregion



#endregion
}
