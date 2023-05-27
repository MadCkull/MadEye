using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

using MadEye.Contracts.ViewModels;
using MadEye.Core.Contracts.Services;
using MadEye.Core.Models;
using MadEye.UserControls;
using MadEye.Views;
using Microsoft.Data.Sqlite;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using static System.Net.Mime.MediaTypeNames;

using System.IO;
using System.Linq;
using WinUIEx.Messaging;
using MadEye.GlobalClasses;

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


    //Controls Number of Loaded Items
    private int BatchSize = 50;
    private int loadedCount = 0;



    #region - Internet History Module:

    //For Passing Values
    public StackPanel HistoryStackContainer { get; set; }
    public Button HistoryLoadButton { get; set; }

    //File Locations
    
    private readonly string HistoryFile = $@"{PathManager.GetInstance().Database}\InternetHistoryDB";

    //Lists to store the Fatched Values
    private readonly List<byte[]> siteIcons = new();
    private readonly List<string> siteTitle = new();
    private readonly List<string> siteUrl = new();
    private readonly List<string> siteVisitTime = new();



    private BitmapImage SetSiteIcon(int Icon_Index)
    {
        var image = new BitmapImage();
        var imageBytes = siteIcons[Icon_Index];
        using (var stream = new InMemoryRandomAccessStream())
        {
            stream.WriteAsync(imageBytes.AsBuffer());
            stream.Seek(0);
            image.SetSourceAsync(stream);
        }
        return image;
    }




    private readonly string selectedDate = ((ShellPage)App.MainWindow.Content).Selected_Date.Replace("\\", "-");

    //Fetches History From Sqllite File and Stores in respactive Lists
    public void GetChromeHistory()
    {
        if (File.Exists(HistoryFile))
        {
            var Table = selectedDate;

            var ConnectionStr = $"Data Source={HistoryFile}";
            using var connection = new SqliteConnection(ConnectionStr);
            connection.Open();
            using var command = new SqliteCommand($"SELECT name FROM sqlite_master WHERE type='table' AND name='{Table}'", connection);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var query = $"SELECT Icon, Title, URL, Time FROM '{Table}'";
                using var command2 = new SqliteCommand(query, connection);
                using var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    var IconStream = reader2.GetStream(0);
                    byte[] IconBytes;
                    using (var ms = new MemoryStream())
                    {
                        IconStream.CopyTo(ms);
                        IconBytes = ms.ToArray();
                    }
                    var title = reader2.GetString(1);
                    var url = reader2.GetString(2);
                    var visitTime = reader2.GetString(3);

                    siteIcons.Add(IconBytes);
                    siteTitle.Add(title);
                    siteUrl.Add(url);
                    siteVisitTime.Add(visitTime);
                }
            }
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
                SiteTimeControl = { Text = siteVisitTime[i] }
            };

            AddHistoryChildToStackPanel(container);
        }

        loadedCount += count;
        Update_HistoryLoadButton();

        TotalEntries = siteTitle.Count.ToString();
    }

    //Adds BrowserHistoryContainer to the HomeDetailPage
    public void AddHistoryChildToStackPanel(UIElement HistoryElement)
    {
        if (HistoryStackContainer != null)
        {
            HistoryStackContainer.Children.Add(HistoryElement);
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



    #region - Keystrokes Capture Module:

    //For Passing Values
    public StackPanel KeystrokesStackContainer { get; set; }
    public Button KeystrokesLoadButton { get; set; }


    //File Locations
    private readonly string KeystrokesFile = $@"{PathManager.GetInstance().Database}\KeystrokesDB";

    //Lists to store the Fatched Values
    //private readonly List<byte[]> WindowIcons = new();
    private readonly List<string> WindowTitle = new();
    private readonly List<string> WindowContent = new();
    private readonly List<string> WindowCaptureTime = new();



    //Fetches Captured Keystrokes From Sqllite File and Stores in respactive Lists
    public void GetCapturedKeystrokes()
    {
        if(File.Exists(KeystrokesFile))
        {
            var Table = selectedDate;

            var ConnectionStr = $@"Data Source={KeystrokesFile}";
            using var connection = new SqliteConnection(ConnectionStr);

            connection.Open();
            using var command = new SqliteCommand($"SELECT name FROM sqlite_master WHERE type='table' AND name='{Table}'", connection);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var query = $"SELECT Title, Content, Time FROM '{Table}' ORDER BY Time DESC";
                using var command2 = new SqliteCommand(query, connection);
                using var reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    //var IconStream = reader2.GetStream(0);
                    //byte[] IconBytes;
                    //using (var ms = new MemoryStream())
                    //{
                    //    IconStream.CopyTo(ms);
                    //    IconBytes = ms.ToArray();
                    //}
                    var title = reader2.GetString(0);       //1
                    var content = reader2.GetString(1);     //2
                    var captureTime = reader2.GetString(2); //3

                    //WindowIcons.Add(IconBytes);
                    WindowTitle.Add(title);
                    WindowContent.Add(content);
                    WindowCaptureTime.Add(captureTime);
                }
            }


        }
    }


    //Sets Values of KeystrokesContainer Controls (Called in Other Classes)
    public void SetKeystrokes()
    {
        var startIndex = loadedCount;
        var count = Math.Min(BatchSize, WindowTitle.Count - startIndex);

        for (var i = startIndex; i < startIndex + count; i++)
        {
            var keystrokesContainer = new KeystrokesContainer
            {
                //WindowIconControl = { ImageSource = SetSiteIcon(i) },
                WindowTitleControl = { Text =  WindowTitle[i] },
                WindowContentControl = { Text = WindowContent[i] },
                WindowTimeControl = { Text = WindowCaptureTime[i] }
            };

            AddKeystrokesChildToStackPanel(keystrokesContainer);
        }

        loadedCount += count;
        Update_KeystrokesLoadButton();

        TotalEntries = WindowTitle.Count.ToString();
    }

    public void AddKeystrokesChildToStackPanel(UIElement KeystrokesElement)
    {
        if (KeystrokesStackContainer != null)
        {
            KeystrokesStackContainer.Children.Add(KeystrokesElement);
        }
    }

    //Updates Keystrokes Load Button
    private void Update_KeystrokesLoadButton()
    {
        KeystrokesLoadButton.IsEnabled = false;

        if (loadedCount == 0)
        {
            KeystrokesLoadButton.Content = "Data Not Found";

        }
        else if (loadedCount == WindowTitle.Count)
        {
            KeystrokesLoadButton.Content = "No More Data";
        }
        else
        {
            KeystrokesLoadButton.Content = "Load More";
            KeystrokesLoadButton.IsEnabled = true;
        }
    }


    #endregion



    #region - Screenshots and WebCam Module

    //For Passing Values
    public GridView ImageStackContainer
    {
        get; set;
    }
    public Button ImageLoadButton
    {
        get; set;
    }


    //List to store the Fatched Screenshots Paths
    private readonly List<string> ImagesPathsList = new();


    //Fetches Captured Screenshots From Folder
    public void GetImages()
    {
        var InitializedModuleID = MadEye.GlobalClasses.GlobalSingletonClass.Instance.SelectedHomeModuleID;
        var pathManager = PathManager.GetInstance();
        pathManager.SelectedDate = ((ShellPage)App.MainWindow.Content).Selected_Date.Replace("\\", "-");
        var folderPath = string.Empty;

        if (InitializedModuleID == 10644)
        {
            pathManager.CheckFolder(pathManager.ScreenshotsSelectedDateFolder);
            folderPath = pathManager.ScreenshotsSelectedDateFolder;
        }
        else if (InitializedModuleID == 10645)
        {
            pathManager.CheckFolder(pathManager.WebCamSelectedDateFolder);
            folderPath = pathManager.WebCamSelectedDateFolder;
        }

        var imageFiles = Directory.GetFiles(folderPath, "*.jpg");
        ImagesPathsList.AddRange(imageFiles);
    }


    //Sets Values of ImageContainer Controls (Called in Other Classes)
    public void SetImages()
    {
        BatchSize = 24;
        var startIndex = loadedCount;
        var count = Math.Min(BatchSize, ImagesPathsList.Count - startIndex);

        for (var i = startIndex; i < startIndex + count; i++)
        {
            var ImageContainer = new ImageContainer
            {
                ImagePath = ImagesPathsList[i]
            };

            AddImageChildToGridView(ImageContainer);
        }

        loadedCount += count;
        Update_ImagesLoadButton();

        TotalEntries = ImagesPathsList.Count.ToString();
    }

    private void AddImageChildToGridView(UIElement ScreenshotsElement)
    {
        if (ImageStackContainer != null)
        {
            ImageStackContainer.Items.Add(ScreenshotsElement);
        }
    }

    //Updates Screenshots Load Button
    private void Update_ImagesLoadButton()
    {
        ImageLoadButton.IsEnabled = false;

        if (loadedCount == 0)
        {
            ImageLoadButton.Content = "Data Not Found";

        }
        else if (loadedCount == ImagesPathsList.Count)
        {
            ImageLoadButton.Content = "No More Data";
        }
        else
        {
            ImageLoadButton.Content = "Load More";
            ImageLoadButton.IsEnabled = true;
        }
    }

    #endregion


#endregion

}
