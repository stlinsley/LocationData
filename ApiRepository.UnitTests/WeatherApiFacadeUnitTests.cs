namespace ApiRepository.UnitTests
{
    using LocationData.Core.Models.Weather;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;
    using Shouldly;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using LocationData.ApiRepository.Facades;
    using Newtonsoft.Json;

    [TestClass]
    public class WeatherApiFacadeUnitTests
    {
        private WeatherApiFacade _weather;
        private Mock<IHttpClientFactory> _clientFactory;
        private Mock<IOptionsMonitor<WeatherDataFacadeOptions>> _options;
        private Mock<WeatherDataFacadeOptions> _weatherOptions;
        private Mock<JsonSerializer> _jsonSerializer;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientFactory = new Mock<IHttpClientFactory>();
            _jsonSerializer = new Mock<JsonSerializer>();
            _options = new Mock<IOptionsMonitor<WeatherDataFacadeOptions>>();
            _weatherOptions = new Mock<WeatherDataFacadeOptions>();
            _weatherOptions.SetupAllProperties();
            _weatherOptions.Object.ApiKey = "test";
            _weatherOptions.Object.BaseUri = "https://test.com";
            _options.Setup(x => x.CurrentValue).Returns(new WeatherDataFacadeOptions());
            _weather = new WeatherApiFacade(_clientFactory.Object, _options.Object, _jsonSerializer.Object);
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
            Should.Throw<ArgumentNullException>(() => { _weather = new WeatherApiFacade(null, _options.Object, _jsonSerializer.Object); });
        }

        [TestMethod]
        public void WeatherApiFacade_OptionsNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _weather = new WeatherApiFacade(_clientFactory.Object, null, _jsonSerializer.Object); });
        }

        [TestMethod]
        public void WeatherApiFacade_JsonSerializerNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _weather = new WeatherApiFacade(_clientFactory.Object, _options.Object, null); });
        }

        #endregion

        #region Method tests

        [TestMethod]
        public async Task GetWeatherDataForLngLat_ReturnsWeatherDataObject()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",

                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        "{'coord': {'lon': '26','lat': '59'},'weather': [{'description': 'broken clouds'}]}")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var response = await _weather.GetWeatherDataForLngLat<WeatherData>(1, 1);
            response.ShouldBeOfType<WeatherData>();
        }

        #endregion
    }
}
