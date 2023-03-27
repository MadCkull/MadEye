using MadEye.Core.Contracts.Services;
using MadEye.Core.Models;

namespace MadEye.Core.Services;

// This class holds sample data used by some generated pages to show how they can be used.
// TODO: The following classes have been created to display sample data. Delete these files once your app is using real data.
// 1. Contracts/Services/ISampleDataService.cs
// 2. Services/SampleDataService.cs
// 3. Models/MainModules.cs
// 4. Models/SampleOrder.cs
// 5. Models/SampleOrderDetail.cs
public class ModuleDataService : ISampleDataService
{
    private List<ModuleProperties> _allOrders;

    public ModuleDataService()
    {
    }

    private static IEnumerable<ModuleProperties> AllOrders()
    {
        // The following is order summary data
        var companies = AllCompanies();
        return companies.SelectMany(c => c.Orders);
    }

    private static IEnumerable<MainModules> AllCompanies()
    {
        return new List<MainModules>()
        {
            new MainModules()
            {
                CompanyName = "KeyStrokes",
                Orders = new List<ModuleProperties>()
                {
                    new ModuleProperties()
                    {
                        ModuleID = 10643, // Used For Navigation
                        SelectedDate = new DateTime(1997, 8, 25),
                        Module = "KeyStrokes",
                        TotalEntries = 814.50,
                        SymbolCode = 57668,
                        SymbolName = "Keyboard",
                        Details = new List<ModulePropertyDetail>()
                        {
                            new ModulePropertyDetail()
                            {
                                Total = 513.00
                            }
                        }
                    }
                }
            },

            new MainModules()
            {
                CompanyName = "Screenshots",
                Orders = new List<ModuleProperties>()
                {
                    new ModuleProperties()
                    {
                       ModuleID = 10644, // Used For Navigation
                        SelectedDate = new DateTime(1997, 8, 25),
                        Module = "Screenshots",
                        TotalEntries = 814.50,
                        SymbolCode = 57688,
                        SymbolName = "Pictures",
                        Details = new List<ModulePropertyDetail>()
                        {
                            new ModulePropertyDetail()
                            {
                                Total = 825.00
                            }
                        }
                    }
                }
            },

            new MainModules()
            {
                CompanyName = "WebCam",
                Orders = new List<ModuleProperties>()
                {
                    new ModuleProperties()
                    {
                        ModuleID = 10645, // Used For Navigation
                        SelectedDate = new DateTime(1997, 8, 25),
                        Module = "WebCam",
                        TotalEntries = 814.50,
                        SymbolCode = 57620,
                        SymbolName = "Camera",
                        Details = new List<ModulePropertyDetail>()
                        {
                            new ModulePropertyDetail()
                            {
                                Total = 380.00
                            }
                        }
                    }
                }
            },


            new MainModules()
            {
                CompanyName = "Internet History",
                Orders = new List<ModuleProperties>()
                {
                    new ModuleProperties()
                    {
                        ModuleID = 10646, // Used For Navigation
                        SelectedDate = new DateTime(1997, 8, 25),
                        Module = "Internet History",
                        TotalEntries = 814.50,
                        SymbolCode = 57633,
                        SymbolName = "Clock",
                        Details = new List<ModulePropertyDetail>()
                        {
                            new ModulePropertyDetail()
                            {
                                Total = 513.00
                            }
                        }
                    }
                }
            },

            new MainModules()
            {
                CompanyName = "File Explorer",
                Orders = new List<ModuleProperties>()
                {
                    new ModuleProperties()
                    {
                       ModuleID = 10647, // Used For Navigation
                        SelectedDate = new DateTime(1997, 8, 25),
                        Module = "File Explorer",
                        TotalEntries = 814.50,
                        SymbolCode = 57736,
                        SymbolName = "Folder",
                        Details = new List<ModulePropertyDetail>()
                        {
                            new ModulePropertyDetail()
                            {
                                Total = 825.00
                            }
                        }
                    }
                }
            },

            new MainModules()
            {
                CompanyName = "Files",
                Orders = new List<ModuleProperties>()
                {
                    new ModuleProperties()
                    {
                        ModuleID = 10648, // Used For Navigation (Not Sure)
                        SelectedDate = new DateTime(1997, 8, 25),
                        Module = "Files",
                        TotalEntries = 814.50,
                        SymbolCode = 57699,
                        SymbolName = "Calendar",
                        Details = new List<ModulePropertyDetail>()
                        {
                            new ModulePropertyDetail()
                            {
                                Total = 380.00
                            }
                        }
                    }
                }
            },


        };
    }

    public async Task<IEnumerable<ModuleProperties>> GetContentGridDataAsync()
    {
        _allOrders ??= new List<ModuleProperties>(AllOrders());

        await Task.CompletedTask;
        return _allOrders;
    }
}
