using MadEye.Activation;
using MadEye.Contracts.Services;
using MadEye.Core.Contracts.Services;
using MadEye.Core.Services;
using MadEye.Helpers;
using MadEye.Models;
using MadEye.Services;
using MadEye.ViewModels;
using MadEye.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.Graphics.Display;
using System.Drawing;

namespace MadEye;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<ISampleDataService, ModuleDataService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<RecWareViewModel>();
            services.AddTransient<RecWarePage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<HomeDetailViewModel>();
            services.AddTransient<HomeDetailPage>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {

       InitializeLoadingPage();
       await Task.Delay(8000);
       window.Hide();




       base.OnLaunched(args);

       await App.GetService<IActivationService>().ActivateAsync(args);
       window.Close();
    }

    private Window window;
    private void InitializeLoadingPage()
    {
        double width = 600;
        double height = 350;

        double screenWidth = 1600;
        double screenHeight = 830;

        double xPos = (screenWidth - width) / 2;
        double yPos = (screenHeight - height) / 2;


        window = new Window();
        window.MoveAndResize(xPos, yPos, width, height);
        window.ExtendsContentIntoTitleBar = true;
        window.SetIsAlwaysOnTop(true);
        window.SetIsResizable(false);
        window.SetIsMaximizable(false);
        window.SetIsMinimizable(false);
        window.SetTitleBarBackgroundColors(Windows.UI.Color.FromArgb(0,1,1,1));
        window.Title = "";
        window.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/LoadScrIcon.ico"));

        window.Activate();

        var loadingScreen = new LoadingScreen();
        window.Content = loadingScreen;
    }

}
