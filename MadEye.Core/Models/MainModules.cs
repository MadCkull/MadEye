namespace MadEye.Core.Models;

// Model for the SampleDataService. Replace with your own model.
public class MainModules
{

    public string CompanyName
    {
        get; set;
    }

    public ICollection<ModuleProperties> Orders
    {
        get; set;
    }
}
