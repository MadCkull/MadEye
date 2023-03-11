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
        ViewModel = App.GetService<HomeDetailViewModel>();
        InitializeComponent();

        GetChromeHistory();
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

    private const string ConnectionStr = @"Data Source=D:\Other\History";
    private const int BatchSize = 50;

    private readonly List<string> siteTitle = new();
    private readonly List<string> siteUrl = new();
    private readonly List<DateTimeOffset> siteVisitTimes = new();

    private int loadedCount = 0;



    private void GetChromeHistory()
    {
        using var connection = new SqliteConnection(ConnectionStr);
        connection.Open();

        var now = DateTimeOffset.UtcNow;
        var yesterday = now.AddDays(-1);
        var yesterdayChromeEpoch = (yesterday.ToUnixTimeSeconds() + 11644473600) * 1000000;
        var nowChromeEpoch = (now.ToUnixTimeSeconds() + 11644473600) * 1000000;
        var query = $"SELECT title, url, last_visit_time FROM urls WHERE last_visit_time BETWEEN {yesterdayChromeEpoch} AND {nowChromeEpoch}";

        using var command = new SqliteCommand(query, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var title = reader.GetString(0);
            var url = reader.GetString(1);
            var visitTimeChromeEpoch = reader.GetInt64(2);
            var visitTimeSeconds = (visitTimeChromeEpoch / 1000000) - 11644473600;
            var visitTime = DateTimeOffset.FromUnixTimeSeconds(visitTimeSeconds);

            if (title == "" || title.Contains("https://"))
            {
                siteTitle.Add("Untitled Window");
            }

            else
            {
                siteTitle.Add(title);
            }
            siteUrl.Add(url);
            siteVisitTimes.Add(visitTime);
        }
    }

    private void FetchHistory()
    {
        var startIndex = loadedCount;
        var count = Math.Min(BatchSize, siteTitle.Count - startIndex);

        for (var i = startIndex; i < startIndex + count; i++)
        {
            var container = new BrowserHistoryContainer
            {
                SiteTitleControl = { Text = siteTitle[i] },
                SiteLinkControl = { Content = siteUrl[i] },
                SiteTimeControl = { Text = siteVisitTimes[i].ToString("h:mm tt") }
            };

            StackContainer.Children.Add(container);
        }

        loadedCount += count;
        UpdateLoadMoreButton();
    }


    private void UpdateLoadMoreButton()
    {
        if (loadedCount == siteTitle.Count)
        {
            btnAddContainer.Content = "No More History";
            btnAddContainer.IsEnabled = false;
        }
        else
        {
            btnAddContainer.Content = "Load More";
            btnAddContainer.IsEnabled = true;
        }

        //TxtData.Text = $"Loaded Data: {loadedCount} | Total Data: {siteTitle.Count}";
    }



    private void Button_Click(object sender, RoutedEventArgs e)
    {
        FetchHistory();
    }
    #endregion



#endregion
}
