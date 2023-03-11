using MadEye.Core.Models;

namespace MadEye.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ISampleDataService
{
    Task<IEnumerable<ModuleProperties>> GetContentGridDataAsync();
}
