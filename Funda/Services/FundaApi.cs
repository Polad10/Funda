using Funda.Constants;
using Funda.Enums;
using Funda.Exceptions;
using Funda.Helpers;
using Funda.Models;
using Funda.Services.Interfaces;

namespace Funda.Services
{
    public class FundaApi : IFundaApi
    {
        private const int PageSize = 25;
        private const int MaxRetries = 5;

        private HttpClient _httpClient;

        public FundaApi(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(FundaApiConstants.FundaHttpClientName);
        }

        public async Task<List<SaleObject>> GetSaleObjects(string city, bool withTuin, Action<int>? progressCallback = null)
        {
            var searchKeys = new List<string>() { city };

            if(withTuin)
            {
                searchKeys.Add("tuin");
            }

            var urlBuilder = new FundaApiUrlBuilder(_httpClient.BaseAddress);
            urlBuilder.SetType(FundaObjectType.Buy);
            urlBuilder.SetSearch(searchKeys);
            urlBuilder.SetPageSize(PageSize);

            var saleObjects = new List<SaleObject>();
            var pageNr = 1;

            SalesData salesData = null;

            do
            {
                salesData = await GetSalesData(pageNr, urlBuilder);
                saleObjects.AddRange(salesData.SaleObjects);

                progressCallback?.Invoke((pageNr * 100) / salesData.Paging.TotalPages);
                pageNr++;
            } while ((pageNr <= salesData.Paging.TotalPages));

            return saleObjects;
        }

        private async Task<SalesData> GetSalesData(int pageNr, FundaApiUrlBuilder urlBuilder)
        {
            int retryNr = 1;

            do
            {
                try
                {
                    urlBuilder.SetPageNr(pageNr);

                    return await _httpClient.GetFromJsonAsync<SalesData>(urlBuilder.AbsoluteUrl);
                }
                catch (HttpRequestException)
                {
                    retryNr++;
                    await Task.Delay(5000);
                }
            } while (retryNr <= MaxRetries);

            throw new RetryLimitExceededException();
        }
    }
}
