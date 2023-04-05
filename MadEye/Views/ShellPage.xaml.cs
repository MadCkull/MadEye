﻿using MadEye.Contracts.Services;
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
        NavigationViewControl.IsPaneOpen = false;
        Shell_Calender.Visibility = Visibility.Collapsed;


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


    public string Selected_Date = $"{DateTime.Now.Day}\\{DateTime.Now.Month}\\{DateTime.Now.Year}";
    


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

#endregion


}
