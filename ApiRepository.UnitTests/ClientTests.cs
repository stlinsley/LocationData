namespace ApiRepository.UnitTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using LocationData.ApiRepository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;
    using Shouldly;

    [TestClass]
    public class ClientTests
    {
        private Client _client;
        private Mock<IHttpClientFactory> _clientFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientFactory = new Mock<IHttpClientFactory>();
            _client = new Client(_clientFactory.Object);
        }

        #region Constructor tests

        [TestMethod]
        public void Client_NullHttpClientFactory_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _client = new Client(null); });
        }

        #endregion

        #region Method tests

        [TestMethod]
        public void CreateClient_ReturnsHttpClient()
        {
            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
            _client.CreateClient().ShouldBeOfType<HttpClient>();
        }

        [TestMethod]
        public async Task SendAsyncRequest_ClientNull_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>( async () =>
                {
                    await _client.SendAsyncRequest(null, new HttpRequestMessage(),
                        HttpCompletionOption.ResponseHeadersRead);
                });
        }

        [TestMethod]
        public async Task SendAsyncRequest_HttpRequestMessageNull_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>( async () =>
            {
                await _client.SendAsyncRequest(new HttpClient(), null,
                    HttpCompletionOption.ResponseHeadersRead);
            });
        }

        [TestMethod]
        public async Task SendAsyncRequest_ValidCall_ReturnsHttpResponseMessage()
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
                    Content = new StringContent("test")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };

            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var response = await _client.SendAsyncRequest(httpClient, new HttpRequestMessage(),
                HttpCompletionOption.ResponseHeadersRead);

            response.ShouldBeOfType<HttpResponseMessage>();
        }

        #endregion
    }
}
