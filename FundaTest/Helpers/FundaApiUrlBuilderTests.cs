using Funda.Enums;
using Funda.Helpers;

namespace FundaTest.Helpers
{
    public class FundaApiUrlBuilderTests
    {
        private readonly Uri _baseUrl;
        private readonly FundaApiUrlBuilder _urlBuilder;

        public FundaApiUrlBuilderTests()
        {
            _baseUrl = new Uri("https://example.com");
            _urlBuilder = new FundaApiUrlBuilder(_baseUrl);
        }

        [Fact]
        public void SetType_SetsCorrectQueryParameterForBuyType()
        {
            var expectedUrl = $"{_baseUrl}?type=koop";

            _urlBuilder.SetType(FundaObjectType.Buy);

            var actualUrl = _urlBuilder.AbsoluteUrl;

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void SetType_SetsCorrectQueryParameterForRentType()
        {
            var expectedUrl = $"{_baseUrl}?type=huur";

            _urlBuilder.SetType(FundaObjectType.Rent);

            var actualUrl = _urlBuilder.AbsoluteUrl;

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void SetSearch_SetsCorrectQueryParameter()
        {
            var key1 = "key1";
            var key2 = "key2";
            var searchKeys = new List<string> { key1, key2 };

            // where %2f = /
            var expectedUrl = $"{_baseUrl}?zo=%2f{key1}%2f{key2}";

            _urlBuilder.SetSearch(searchKeys);

            var actualUrl = _urlBuilder.AbsoluteUrl;

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void SetPageNr_SetsCorrectQueryParameter()
        {
            var pageNr = 5;
            var expectedUrl = $"{_baseUrl}?page={pageNr}";

            _urlBuilder.SetPageNr(pageNr);

            var actualUrl = _urlBuilder.AbsoluteUrl;

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void SetPageSize_SetsCorrectQueryParameter()
        {
            var pageSize = 10;
            var expectedUrl = $"{_baseUrl}?pagesize={pageSize}";

            _urlBuilder.SetPageSize(pageSize);

            var actualUrl = _urlBuilder.AbsoluteUrl;

            Assert.Equal(expectedUrl, actualUrl);
        }
    }
}
