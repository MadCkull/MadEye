// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MadEye.UserControls;
public sealed partial class KeystrokesContainer : UserControl
{
    public KeystrokesContainer()
    {
        this.InitializeComponent();

        LayoutRootGrid.Background = (Brush)BackgroundAcrylicBrush(0.2, 0.4, 0.2);
    }


    public Microsoft.UI.Xaml.Media.ImageBrush WindowIconControl => Window_Icon;

    public TextBlock WindowTitleControl => Window_Title;

    public TextBlock WindowContentControl => Window_Content;

    public TextBlock WindowTimeControl => Window_Time;

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

        if (LayoutRootGrid.Height != 46.5 && Window_Content.ActualHeight > 30)
        {
            ExpendSymbol.Text = "\u25BE";
            LayoutRootGrid.Height = 46.5;
        }
        else if(Window_Content.ActualHeight > 30)
        {
            ExpendSymbol.Text = "\u25B5";
            LayoutRootGrid.Height = Window_Content.Height;
        }
    }


    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        LayoutRootGrid.Background = (Brush)BackgroundAcrylicBrush(0.3, 0.6, 0.2);
    }

    private void OnContainer_Loaded(object sender, RoutedEventArgs e)
    {
        if (Window_Content.ActualHeight >30)
        {
            ExpendSymbol.Text = "\u25BE";
        }
    }
}
