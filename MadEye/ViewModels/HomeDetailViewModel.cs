﻿using CommunityToolkit.Mvvm.ComponentModel;

using MadEye.Contracts.ViewModels;
using MadEye.Core.Contracts.Services;
using MadEye.Core.Models;
using MadEye.Views;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace MadEye.ViewModels;

public class HomeDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private ModuleProperties? _item;

    public ModuleProperties? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    
    public HomeDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.ModuleID == orderID);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public StackPanel HistoryStackContainer { get; set; }
    public Button HistoryLoadButton { get; set; }
    public string TotalEntries { get; set; }



    private const string ConnectionStr = @"Data Source=D:\Other\History";
    private readonly List<string> siteTitle = new();
    private readonly List<string> siteUrl = new();
    private readonly List<DateTimeOffset> siteVisitTimes = new();
    private const int BatchSize = 50;
    private int loadedCount = 0;



    public static DateTimeOffset DataParse()
    {
        var selectedDate = ((ShellPage)App.MainWindow.Content).Selected_Date;
        var dateParts = selectedDate.Split('\\');
        var day = int.Parse(dateParts[0]);
        var month = int.Parse(dateParts[1]);
        var year = int.Parse(dateParts[2]);

        DateTimeOffset date = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        return date;
    }



    public void GetChromeHistory()
    {
        DateTimeOffset date = DataParse();

        using var connection = new SqliteConnection(ConnectionStr);
        connection.Open();

        var startOfDay = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, TimeSpan.Zero);
        var endOfDay = startOfDay.AddDays(1).AddTicks(-1);
        var startOfDayUnixTime = (startOfDay.UtcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        var endOfDayUnixTime = (endOfDay.UtcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        var startOfDayChromeEpoch = (startOfDayUnixTime + 11644473600) * 1000000;
        var endOfDayChromeEpoch = (endOfDayUnixTime + 11644473600) * 1000000;
        var query = $"SELECT title, url, last_visit_time FROM urls WHERE last_visit_time BETWEEN {startOfDayChromeEpoch} AND {endOfDayChromeEpoch} ORDER BY last_visit_time ASC";

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


    public void FetchHistory()
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

            AddChildToStackPanel(container);
        }

        loadedCount += count;
        Update_HistoryLoadButton();

        TotalEntries = siteTitle.Count.ToString();
    }

    public void AddChildToStackPanel(UIElement element)
    {
        if (HistoryStackContainer != null)
        {
            HistoryStackContainer.Children.Add(element);
        }
    }



    private void Update_HistoryLoadButton()
    {
        HistoryLoadButton.IsEnabled = false;

        if (loadedCount == 0)
        {
            HistoryLoadButton.Content = "Data Not Found";
            
        }
        else if (loadedCount == siteTitle.Count)
        {
            HistoryLoadButton.Content = "No More Data";
        }
        else
        {
            HistoryLoadButton.Content = "Load More";
            HistoryLoadButton.IsEnabled = true;
        }
    }


}
