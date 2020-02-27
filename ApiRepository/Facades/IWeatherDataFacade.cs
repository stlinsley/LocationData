namespace LocationData.ApiRepository.Facades
{
    using System.Threading.Tasks;

    public interface IWeatherDataFacade
    {
        Task<T> GetWeatherDataForLngLat<T>(decimal lng, decimal lat);
    }
}