// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MadEye;

public sealed partial class BrowserHistoryContainer : UserControl
{
    public BrowserHistoryContainer()
    {
        this.InitializeComponent();

        LayoutRootGrid.Background = (Brush)BackgroundAcrylicBrush(0.2, 0.4, 0.2);
    }


    public TextBlock SiteTitleControl => Site_Title;

    public HyperlinkButton SiteLinkControl => Site_Link;

    public TextBlock SiteTimeControl => Site_Time;


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

    private async void Site_Link_Click(object sender, RoutedEventArgs e)
    {
        // Launch the default browser with the specified URI
        await Launcher.LaunchUriAsync(new Uri(uriString: Site_Link.Content.ToString()));
    }

    private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {

        LayoutRootGrid.Background = (Brush)BackgroundAcrylicBrush(0.3, 0.6, 0.2);
    }

    private void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {

        LayoutRootGrid.Background = (Brush)BackgroundAcrylicBrush(0.2, 0.4, 0.2);
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        LayoutRootGrid.Background = (Brush)BackgroundAcrylicBrush(0.1, 0.2, 0.2);
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        LayoutRootGrid.Background = (Brush)BackgroundAcrylicBrush(0.3, 0.6, 0.2);
    }
}
