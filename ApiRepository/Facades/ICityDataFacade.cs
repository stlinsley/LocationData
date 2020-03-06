namespace LocationData.ApiRepository.Facades
{
    using System.Threading.Tasks;

    public interface ICityDataFacade
    {
        Task<T> GetCityData<T>(string city);
    }
}