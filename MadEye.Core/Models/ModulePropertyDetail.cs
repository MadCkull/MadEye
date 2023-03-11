namespace MadEye.Core.Models;

// Model for the SampleDataService. Replace with your own model.
public class ModulePropertyDetail
{

    public string ProductName
    {
        get; set;
    }

    public int Quantity
    {
        get; set;
    }

    public string CategoryName
    {
        get; set;
    }

    public double Total
    {
        get; set;
    }

    public string ShortDescription => $"Product ID: {ProductName}";
}
