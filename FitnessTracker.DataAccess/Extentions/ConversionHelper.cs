namespace FitnessTracker.DataAccess.Extentions;

public static class ConversionHelper
{
    public static Guid StringToGuid(string v)
    {
        return Guid.TryParse(v, out var guid) ? guid : Guid.Empty;
    }
}
