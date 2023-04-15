using MadEye.Helpers;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Storage;
using WinUIEx;

namespace MadEye;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }


    public void ChangeBackdrop()
    {
        //This is just Dummy Code for startup
        this.Backdrop = new AcrylicSystemBackdrop();
        this.Backdrop = new MicaSystemBackdrop();
    }


    #region > User Defined:



    #endregion

}
