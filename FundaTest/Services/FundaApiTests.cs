using Funda.Constants;
using Funda.Enums;
using Funda.Models;
using Funda.Services;
using Moq;
using System.Text.Json;
using RichardSzalay.MockHttp;
using System.Net;
using Funda.Exceptions;

namespace FundaTest.Services
{
    public class FundaApiTests
    {
        private readonly MockHttpMessageHandler _httpMock;

        private readonly Uri _baseAddress;
        private readonly FundaApi _fundaApi;

        public FundaApiTests()
        {
            _httpMock = new MockHttpMessageHandler();

            var httpClientFactory = new Mock<IHttpClientFactory>();

            _baseAddress = new Uri("https://example.com");
            var httpClient = _httpMock.ToHttpClient();
            httpClient.BaseAddress = _baseAddress;

            httpClientFactory.Setup(c => c.CreateClient(FundaApiConstants.FundaHttpClientName))
                .Returns(httpClient);

            _fundaApi = new FundaApi(httpClientFactory.Object);
            _fundaApi.SetDelayBetweenRetries(0);
        }

        [Fact]
        public async Task GetSaleObjects_ReturnsAllSaleObjectsFromApi()
        {
            var pagingData = new Paging() { TotalPages = 2 };
            
            var agentName1 = "Agent1";
            var agentName2 = "Agent2";
            var saleObject1 = new SaleObject() { AgentName = agentName1 };
            var saleObject2 = new SaleObject() { AgentName = agentName2 };

            var salesData1 = new SalesData()
            {
                Paging = pagingData,
                SaleObjects = new List<SaleObject> { saleObject1 }
            };

            var salesData2 = new SalesData()
            {
                Paging = pagingData,
                SaleObjects = new List<SaleObject> { saleObject2 }
            };

            var json1 = JsonSerializer.Serialize(salesData1);
            var json2 = JsonSerializer.Serialize(salesData2);

            _httpMock.When("https://example.com/?type=koop&zo=%2f&pagesize=25&page=1").Respond("application/json", json1);
            _httpMock.When("https://example.com/?type=koop&zo=%2f&pagesize=25&page=2").Respond("application/json", json2);

            var saleObjects = await _fundaApi.GetSaleObjects("", FundaObjectType.Buy, false, CancellationToken.None);

            Assert.Equal(2, saleObjects.Count);
            Assert.Equal(agentName1, saleObjects[0].AgentName);
            Assert.Equal(agentName2, saleObjects[1].AgentName);
        }

        [Fact]
        public async Task GetSaleObjects_WhenCancelled_ThrowsOperationCanceledException()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _fundaApi.GetSaleObjects("", FundaObjectType.Buy, false, cancellationTokenSource.Token));
        }

        [Fact]
        public async Task GetSaleObjects_WhenNumberOfRetriesExceeds_ThrowsRetryLimitExceededException()
        {
            _httpMock.When("https://example.com/?type=koop&zo=%2f&pagesize=25&page=1").Respond(req => new HttpResponseMessage(HttpStatusCode.Unauthorized));

            await Assert.ThrowsAsync<RetryLimitExceededException>(async () => await _fundaApi.GetSaleObjects("", FundaObjectType.Buy, false, CancellationToken.None));
        }
    }
}
