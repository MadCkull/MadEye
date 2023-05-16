// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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



    public string ScreenshotPathControl;

    
    private void SetScreenshotPath()
    {
        Screenshot.Source = new BitmapImage(new Uri(@"D:\FYP\Screenshots\MadEye - 11-51 PM.jpg"));
    }



}
