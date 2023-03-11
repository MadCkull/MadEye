namespace MadEye.Core.Models;

// Model for the SampleDataService. Replace with your own model.
public class ModuleProperties
{

    public long ModuleID
    {
        get; set;
    }


    public DateTime SelectedDate
    {
        get; set;
    }

    public string Module
    {
        get; set;
    }

    public double TotalEntries
    {
        get; set;
    }

    public int SymbolCode
    {
        get; set;
    }

    public string SymbolName
    {
        get; set;
    }

    public char Symbol => (char)SymbolCode;

    public ICollection<ModulePropertyDetail> Details
    {
        get; set;
    }

    public override string ToString() => $"{Module}";
}