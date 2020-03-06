namespace ApiRepository.UnitTests
{
    using LocationData.ApiRepository;
    using LocationData.ApiRepository.Facades;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Shouldly;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using LocationData.Core.Models.Weather;

    [TestClass]
    public class WeatherApiFacadeUnitTests
    {
        private WeatherApiFacade _weather;
        private Mock<IClient> _client;
        private Mock<IOptionsMonitor<WeatherDataFacadeOptions>> _options;
        private Mock<WeatherDataFacadeOptions> _weatherOptions;
        private Mock<ISerialization> _serialization;

        [TestInitialize]
        public void TestInitialize()
        {
            _client = new Mock<IClient>();
            _options = new Mock<IOptionsMonitor<WeatherDataFacadeOptions>>();
            _serialization = new Mock<ISerialization>();
            _weatherOptions = new Mock<WeatherDataFacadeOptions>();
            _weatherOptions.SetupAllProperties();
            _weatherOptions.Object.ApiKey = "test";
            _weatherOptions.Object.BaseUri = "https://test.com";
            _options.Setup(x => x.CurrentValue).Returns(new WeatherDataFacadeOptions());
            _weather = new WeatherApiFacade(_client.Object, _options.Object, _serialization.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mock.VerifyAll();
        }

        #region Constructor tests

        [TestMethod]
        public void WeatherApiFacade_HttpClientFactoryNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _weather = new WeatherApiFacade(null, _options.Object, _serialization.Object); });
        }

        [TestMethod]
        public void WeatherApiFacade_OptionsNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _weather = new WeatherApiFacade(_client.Object, null, _serialization.Object); });
        }

        [TestMethod]
        public void WeatherApiFacade_SerializationNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _weather = new WeatherApiFacade(_client.Object, _options.Object, null); });
        }

        #endregion

        #region Method tests

        [TestMethod]
        public async Task GetWeatherDataForLngLat_ReturnsWeatherDataObject()
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

            _serialization.Setup(s => s.Deserialize<WeatherData>(It.IsAny<HttpResponseMessage>())).Returns(Task.Run(() => new WeatherData()));


            var response = await _weather.GetWeatherDataForLngLat<WeatherData>(1, 1);
            response.ShouldBeOfType<WeatherData>();
        }

        #endregion
    }
}
