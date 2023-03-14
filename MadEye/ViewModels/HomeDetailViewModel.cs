using CommunityToolkit.Mvvm.ComponentModel;

using MadEye.Contracts.ViewModels;
using MadEye.Core.Contracts.Services;
using MadEye.Core.Models;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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






    //My Code:


    private StackPanel _HistoryStackContainer;
    public StackPanel HistoryStackContainer
    {
        get => _HistoryStackContainer;
        set => _HistoryStackContainer = value;
    }

    private Button _HistoryLoadButton;
    public Button HistoryLoadButton
    {
        get => _HistoryLoadButton;
        set => _HistoryLoadButton = value;
    }

    private const string ConnectionStr = @"Data Source=D:\Other\History";
    private readonly List<string> siteTitle = new();
    private readonly List<string> siteUrl = new();
    private readonly List<DateTimeOffset> siteVisitTimes = new();
    private const int BatchSize = 50;
    private int loadedCount = 0;



public void GetChromeHistory()
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
    }

    public void AddChildToStackPanel(UIElement element)
    {
        if (_HistoryStackContainer != null)
        {
            _HistoryStackContainer.Children.Add(element);
        }
    }



    private void Update_HistoryLoadButton()
    {
        if (loadedCount == siteTitle.Count)
        {
            _HistoryLoadButton.Content = "No More History";
            _HistoryLoadButton.IsEnabled = false;
        }
        else
        {
            _HistoryLoadButton.Content = "Load More";
            _HistoryLoadButton.IsEnabled = true;
        }
    }


}
