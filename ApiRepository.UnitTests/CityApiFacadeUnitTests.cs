namespace ApiRepository.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using LocationData.ApiRepository.Facades;
    using LocationData.Core.Models.City;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;
    using Shouldly;

    [TestClass]
    public class CityApiFacadeUnitTests
    {
        private CityApiFacade _city;
        private Mock<IHttpClientFactory> _clientFactory;
        private Mock<IOptionsMonitor<CityDataFacadeOptions>> _options;
        private Mock<CityDataFacadeOptions> _cityOptions;
        private Mock<JsonSerializer> _jsonSerializer;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientFactory = new Mock<IHttpClientFactory>();
            _options = new Mock<IOptionsMonitor<CityDataFacadeOptions>>();
            _cityOptions = new Mock<CityDataFacadeOptions>();
            _cityOptions.SetupAllProperties();
            _cityOptions.Object.BaseUri = "https://test.com";
            _jsonSerializer = new Mock<JsonSerializer>();
            _options.Setup(x => x.CurrentValue).Returns(new CityDataFacadeOptions());
            _city = new CityApiFacade(_clientFactory.Object, _options.Object, _jsonSerializer.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Mock.VerifyAll();
        }

        #region Constructor tests

        [TestMethod]
        public void CityApiFacade_HttpClientFactoryNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _city = new CityApiFacade(null, _options.Object, _jsonSerializer.Object); });
        }

        [TestMethod]
        public void CityApiFacade_OptionsNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _city = new CityApiFacade(_clientFactory.Object, null, _jsonSerializer.Object); });
        }

        [TestMethod]
        public void CityApiFacade_JsonSerializerNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _city = new CityApiFacade(_clientFactory.Object, _options.Object, null); });
        }

        #endregion

        #region Method tests

        [TestMethod]
        public async Task GetCityData_CityNull_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await _city.GetCityData<City>(null);
            });
        }

        [TestMethod]
        public async Task GetCityData_CityEmpty_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await _city.GetCityData<City>("");
            });
        }

        [TestMethod]
        public async Task GetCityData_ReturnsListOfCity()
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
                        "[{'name': 'United Kingdom of Great Britain and Northern Ireland','alpha2Code': 'GB','alpha3Code': 'GBR','capital': 'London'," +
                        "'population': 65110000,'latlng': [54,-2],'currencies': [{'code': 'GBP','name': 'British pound','symbol': '£'}]}]")});

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var response = await _city.GetCityData<City>("london");
            response.ShouldBeOfType<List<City>>();
        }

        #endregion
    }
}
