using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MadEye.Contracts.Services;
using MadEye.Contracts.ViewModels;
using MadEye.Core.Contracts.Services;
using MadEye.Core.Models;
using MadEye.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Windows.ApplicationModel.DataTransfer;

using MadEye.GlobalClasses;

namespace MadEye.ViewModels;

public class HomeViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly ISampleDataService _sampleDataService;

    public ICommand ItemClickCommand
    {
        get;
    }

    public ObservableCollection<ModuleProperties> Source { get; } = new ObservableCollection<ModuleProperties>();

    public HomeViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
    {
        _navigationService = navigationService;
        _sampleDataService = sampleDataService;

        ItemClickCommand = new RelayCommand<ModuleProperties>(OnItemClick);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await _sampleDataService.GetContentGridDataAsync();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {

    }

    
    private void OnItemClick(ModuleProperties? clickedItem)
    {

        if (clickedItem != null)
        {
            GlobalClasses.GlobalSingletonClass.Instance.HomeModuleSelectedModuleID = clickedItem.ModuleID;

            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(HomeDetailViewModel).FullName!, clickedItem.ModuleID);
        }

        if (clickedItem != null && clickedItem.ModuleID == 10643)
        {
            //My Code *.*
        }
    }
    

}
