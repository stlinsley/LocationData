namespace LocationData.Core.Models.Weather
{
    public interface ICoord
    {
        double Lat { get; set; }
        double Lon { get; set; }
    }
}