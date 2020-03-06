namespace LocationData.Core.Models.City
{
    public interface ICurrency
    {
        int CurrencyId { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
    }
}