using MadEye.Contracts.Services;
using MadEye.Helpers;
using MadEye.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Media.Devices;
using Windows.System;
using System.Windows.Input;
using Windows.Security.Cryptography.Certificates;
using Windows.UI.Core;
using WindowActivatedEventArgs = Microsoft.UI.Xaml.WindowActivatedEventArgs;
using MadEye.Services;
using Windows.ApplicationModel.DataTransfer;
using MadEye.Core.Models;
using Windows.UI.Popups;
using Windows.ApplicationModel.Email;
using WinUIEx.Messaging;
using System.ComponentModel.Design;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using CommunityToolkit.WinUI.UI;
using MadEye.GlobalClasses;
using static System.Net.WebRequestMethods;
using System.IO.Compression;
using System.Collections.ObjectModel;

namespace MadEye.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{

#region > Template Code (Do NOT Modify)

    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        UserList.Clear();
        GetUsers(@"D:\FYP");

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();

        App.MainWindow.IsResizable = false;
        NavigationViewControl.IsPaneOpen = false; //Closes Navigation Panel
        Shell_Calender.Visibility = Visibility.Collapsed; //Hides Calander when Navigation Panel is Closed

    }

    private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush)App.Current.Resources[resource];
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

#endregion


#region   > User Defined:

    public string Selected_Date = $"{DateTime.Now.Day}\\{DateTime.Now.Month}\\{DateTime.Now.Year}";

    private void NavigationViewControl_PaneClosing(NavigationView sender, object args)
    {
        Shell_Calender.Visibility = Visibility.Collapsed;
    }

    private void NavigationViewControl_PaneOpening(NavigationView sender, object args)
    {
        Shell_Calender.Visibility = Visibility.Visible;
    }


    private void Shell_Calender_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        Shell_Calender.IsSelected = true;
    }

    private void Shell_Calender_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        Shell_Calender.IsSelected = false;
    }


    private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
    {
        INavigationService navigationService = App.GetService<INavigationService>();
        navigationService.NavigateTo("MadEye.ViewModels.HomeViewModel");

        Shell_MadEye.IsSelected = true;
        NavigationViewControl.IsPaneOpen = false;

        //Gets the first (and only) selected date
        var selectedDate = sender.SelectedDates.FirstOrDefault();

        Selected_Date = $"{selectedDate.Day}\\{selectedDate.Month}\\{selectedDate.Year}";
    }




    public List<string> UserList { get; } = new List<string>();

    public void GetUsers(string directoryPath)
    {
        UserList.Clear();

        if (Directory.Exists(directoryPath))
        {


            
            string[] folderPaths = Directory.GetDirectories(directoryPath);

            foreach (string folderPath in folderPaths)
            {
                string folderName = new DirectoryInfo(folderPath).Name;
                if (folderName != null)
                {
                    UserList.Add(folderName);
                }
                
            }
        }
    }


    private void ExtractUserData()
    {
        var sourceDirectory = @"D:\UserData";
        var destinationDirectory = PathManager.GetInstance().HomeDirectory;

        var zipFiles = Directory.GetFiles(sourceDirectory, "*.zip");

        if (zipFiles.Length > 0)
        {

            try
            {
                foreach (var zipFile in zipFiles)
                {
                    ZipFile.ExtractToDirectory(zipFile, destinationDirectory + Path.GetFileNameWithoutExtension(zipFile), true);

                    System.IO.File.Delete(zipFile);
                }
            }
            catch (Exception)
            {
            }
        }

        GetUsers(@"D:\FYP");

    }





    public string SelectedUser = "DummyUser";
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        SelectedUser = comboBox.SelectedItem as string;

        if (SelectedUser != null)
        {
            PathManager.GetInstance().UpdateUsername(SelectedUser);
        }
    }



    private ObservableCollection<string> userList = new ObservableCollection<string>();
    private ComboBox loadedComboBox;

    private void ComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        loadedComboBox = (ComboBox)sender;
        loadedComboBox.ItemsSource = userList;
        UpdateUsersList();
    }

    private void UpdateUsersList()
    {
        // Remove items not in UserList
        var itemsToRemove = userList.Except(UserList).ToList();
        foreach (var item in itemsToRemove)
        {
            userList.Remove(item);
        }

        // Add items from UserList
        var itemsToAdd = UserList.Except(userList).ToList();
        foreach (var item in itemsToAdd)
        {
            userList.Add(item);
        }

        // Select first item if available
        if (userList.Count > 0)
        {
            loadedComboBox.SelectedItem = userList[0];
        }
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        ExtractUserData();
        UpdateUsersList();
    }















    #endregion


}
