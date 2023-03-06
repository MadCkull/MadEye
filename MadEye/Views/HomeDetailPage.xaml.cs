using CommunityToolkit.WinUI.UI.Animations;

using MadEye.Contracts.Services;
using MadEye.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace MadEye.Views;

public sealed partial class HomeDetailPage : Page
{
    public HomeDetailViewModel ViewModel
    {
        get;
    }

    public HomeDetailPage()
    {
        ViewModel = App.GetService<HomeDetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
