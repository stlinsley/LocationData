namespace LocationData.Core.Models.City
{
    public interface ICurrency
    {
        string Code { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
    }
}