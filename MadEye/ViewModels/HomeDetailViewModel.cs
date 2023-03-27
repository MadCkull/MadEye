using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.Mvvm.ComponentModel;

using MadEye.Contracts.ViewModels;
using MadEye.Core.Contracts.Services;
using MadEye.Core.Models;
using MadEye.Views;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using static System.Net.Mime.MediaTypeNames;

namespace MadEye.ViewModels;

public class HomeDetailViewModel : ObservableRecipient, INavigationAware
{

#region > Template Code (Do NOT Modify)

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

    #endregion



#region > User Defined:

    public string TotalEntries
    {
        get; set;
    }



    #region - Internet History Module:

    //For Passing Values
    public StackPanel HistoryStackContainer
    {
        get; set;
    }
    public Button HistoryLoadButton
    {
        get; set;
    }

    //File Locations
    private readonly string FaviconFile = @"D:\Other\Favicons";
    private readonly string HistoryFile = @"Data Source=D:\Other\History";
    
    //Lists to store the Fatched Values
    private readonly List<byte[]> siteIcons = new();
    private readonly List<string> siteTitle = new();
    private readonly List<string> siteUrl = new();
    private readonly List<DateTimeOffset> siteVisitTimes = new();

    //Controls Number of Loaded Items
    private const int BatchSize = 50;
    private int loadedCount = 0;


    //Convert Date to Chrome's Format
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


    //Fetches Favicons From Sqllite File
    private byte[] GetFavicons(string url)
    {
        var siteUri = new Uri(url);
        string siteUrl = siteUri.GetLeftPart(UriPartial.Authority);

        var iconPath = FaviconFile;
        if (!File.Exists(iconPath)) return null;

        byte[] faviconData = null;
        using (var connection = new SqliteConnection($"Data Source={iconPath};"))
        {
            connection.Open();
            using (var command = new SqliteCommand("SELECT icon_mapping.page_url, favicon_bitmaps.image_data FROM icon_mapping JOIN favicon_bitmaps ON icon_mapping.icon_id = favicon_bitmaps.icon_id WHERE icon_mapping.page_url LIKE '%' || @url || '%'", connection))
            {
                command.Parameters.AddWithValue("@url", siteUrl);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        faviconData = GetBytes(reader);
                    }
                }
            }
        }

        return faviconData;
    }

    //Converts Fatches Icons (PNG) to Byte[]
    private byte[] GetBytes(SqliteDataReader reader)
    {
        const int CHUNK_SIZE = 2 * 1024;
        var buffer = new byte[CHUNK_SIZE];
        long bytesRead;
        long fieldOffset = 0;
        using (MemoryStream stream = new MemoryStream())
        {
            while ((bytesRead = reader.GetBytes(1, fieldOffset, buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, (int)bytesRead);
                fieldOffset += bytesRead;
            }
            return stream.ToArray();
        }
    }

    //Stores  Byte[] in list
    private void StoreFavicon(string url)
    {
        var faviconData = GetFavicons(url);
        if (faviconData != null)
        {
            siteIcons.Add(faviconData);
        }
        else
        {
            StorageFile file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Resources/DefaultSiteIcon.png")).AsTask().Result;
            IBuffer buffer = FileIO.ReadBufferAsync(file).AsTask().Result;
            byte[] DefaultIcon = buffer.ToArray();
            siteIcons.Add(DefaultIcon);
        }
    }

    //Returns Image based on the URL Index
    private BitmapImage SetSiteIcon(int Url_Index)
    {
        var image = new BitmapImage();
        if (Url_Index >= 0 && Url_Index < siteIcons.Count)
        {
            
            var imageBytes = siteIcons[Url_Index];
            using (var stream = new InMemoryRandomAccessStream())
            {
                stream.WriteAsync(imageBytes.AsBuffer());
                stream.Seek(0);
                image.SetSourceAsync(stream);
            }
            return image;
        }
        else
        {
            image = new BitmapImage(new Uri("ms-appx:///MadEye/Resources/DefaultSiteIcon.png"));
            return image;
        }
    }


    //Fetches History From Sqllite File and Stores in respactive Lists
    public void GetChromeHistory()
    {
        DateTimeOffset date = DataParse();

        string ConnectionStr = HistoryFile;
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

            StoreFavicon(url);

            if (title == "" || title.Contains("https://"))
            {
                siteTitle.Add("Untitled Page");
            }
            else
            {
                siteTitle.Add(title);
            }
            siteUrl.Add(url);
            siteVisitTimes.Add(visitTime);
        }
    }

    //Sets Values of BrowserHistoryContainer Controls
    public void SetHistory()
    {
        var startIndex = loadedCount;
        var count = Math.Min(BatchSize, siteTitle.Count - startIndex);

        for (var i = startIndex; i < startIndex + count; i++)
        {
            var container = new BrowserHistoryContainer
            {
                SiteIconControl = { ImageSource = SetSiteIcon(i) },
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

    //Adds BrowserHistoryContainer to the HomeDetailPage
    public void AddChildToStackPanel(UIElement element)
    {
        if (HistoryStackContainer != null)
        {
            HistoryStackContainer.Children.Add(element);
        }
    }

    //Updates History Load Button
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

    #endregion

#endregion

}
