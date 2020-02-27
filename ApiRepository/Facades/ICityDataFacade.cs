namespace LocationData.ApiRepository.Facades
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICityDataFacade
    {
        Task<List<T>> GetCityData<T>(string city);
    }
}