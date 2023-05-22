// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ABI.Windows.UI;
using MadEye.Views;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MadEye.UserControls;
public sealed partial class ScreenshotsContainer : UserControl
{

    public ScreenshotsContainer()
    {
        this.InitializeComponent();
    }

    private string screenshotPathControl;
    public string ScreenshotPathControl
    {
        get
        {
            return screenshotPathControl;
        }
        set
        {
            screenshotPathControl = value;
            SetScreenshotPath();
        }
    }

    private object BackgroundAcrylicBrush(double Opacity, double TintOpacity, double TintLuminosityOpacity)
    {
        AcrylicBrush acrylicBrush = new AcrylicBrush()
        {
            Opacity = Opacity,
            TintOpacity = TintOpacity,
            TintLuminosityOpacity = TintLuminosityOpacity,

            TintColor = Colors.Transparent,
            FallbackColor = Colors.White

        };

        return acrylicBrush;
    }




    private void SetScreenshotPath()
    {
        Title.Text = Path.GetFileName(ScreenshotPathControl.Replace(".jpg", null));
        Screenshot.Source = new BitmapImage(new Uri(ScreenshotPathControl));
    }

    private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        TitleContainer.Background = (Brush)BackgroundAcrylicBrush(0.3, 0.6, 0.2);
    }

    private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        TitleContainer.Background = (Brush)BackgroundAcrylicBrush(0, 0, 0);
    }







    private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        // Find the parent HomeDetailPage of the ScreenshotContainer
        HomeDetailPage homeDetailPage = FindParent<HomeDetailPage>(this);

        homeDetailPage.PreviewScreenshot(screenshotPathControl);
    }

    // Recursive method to find the parent of a specific type in the Visual Tree
    private T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        // Get the parent of the child element
        var parent = VisualTreeHelper.GetParent(child);

        // Check if the parent is of the specified type
        if (parent is T typedParent)
        return typedParent;

        // Recursively call FindParent on the parent element
        return FindParent<T>(parent);
    }

}

