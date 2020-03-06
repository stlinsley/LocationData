namespace ApiRepository.UnitTests
{
    using LocationData.ApiRepository;
    using LocationData.ApiRepository.Facades;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using LocationData.Core.Models.City;

    [TestClass]
    public class CityApiFacadeUnitTests
    {
        private CityApiFacade _city;
        private Mock<IClient> _client;
        private Mock<IOptionsMonitor<CityDataFacadeOptions>> _options;
        private Mock<CityDataFacadeOptions> _cityOptions;
        private Mock<ISerialization> _serialization;

        [TestInitialize]
        public void TestInitialize()
        {
            _client = new Mock<IClient>();
            _options = new Mock<IOptionsMonitor<CityDataFacadeOptions>>();
            _serialization = new Mock<ISerialization>();
            _cityOptions = new Mock<CityDataFacadeOptions>();
            _cityOptions.SetupAllProperties();
            _cityOptions.Object.BaseUri = "https://test.com";
            _options.Setup(x => x.CurrentValue).Returns(new CityDataFacadeOptions());
            _city = new CityApiFacade(_client.Object, _options.Object, _serialization.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mock.VerifyAll();
        }

        #region Constructor tests

        [TestMethod]
        public void CityApiFacade_ClientFactoryNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _city = new CityApiFacade(null, _options.Object, _serialization.Object); });
        }

        [TestMethod]
        public void CityApiFacade_OptionsNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _city = new CityApiFacade(_client.Object, null, _serialization.Object); });
        }

        [TestMethod]
        public void CityApiFacade_SeralizationNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _city = new CityApiFacade(_client.Object, _options.Object, null); });
        }

        #endregion

        #region Method tests

        [TestMethod]
        public async Task GetCityData_CityNull_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await _city.GetCityData<List<City>>(null);
            });
        }

        [TestMethod]
        public async Task GetCityData_CityEmpty_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await _city.GetCityData<List<City>>("");
            });
        }

        [TestMethod]
        public async Task GetCityData_ReturnsListOfCity()
        {

            _client.Setup(x => x.CreateClient()).Returns(new HttpClient());

            _client.Setup(x => x.SendAsyncRequest(
                It.IsAny<HttpClient>(),
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<HttpCompletionOption>()
            )).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
                }
            );

            _serialization.Setup(s => s.Deserialize<List<City>>(It.IsAny<HttpResponseMessage>())).Returns(Task.Run(() => new List<City>()));

            var response = await _city.GetCityData<List<City>>("london");
            
            response.ShouldBeOfType<List<City>>();
        }

        #endregion
    }
}
