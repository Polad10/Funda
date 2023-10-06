using Funda.Constants;
using Funda.Enums;
using Funda.Helpers;
using Funda.Models;
using Funda.Services.Interfaces;

namespace Funda.Services
{
    public class FundaApi : IFundaApi
    {
        private const int PageSize = 25;

        private IHttpClientFactory _httpClientFactory;

        public FundaApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<SaleObject>> GetSaleObjects(string city, bool withTuin, Action<int>? progressCallback = null)
        {
            var searchKeys = new List<string>() { city };

            if(withTuin)
            {
                searchKeys.Add("tuin");
            }

            var httpClient = _httpClientFactory.CreateClient(FundaApiConstants.FundaHttpClientName);

            var urlBuilder = new FundaApiUrlBuilder(httpClient.BaseAddress);
            urlBuilder.SetType(FundaObjectType.Buy);
            urlBuilder.SetSearch(searchKeys);
            urlBuilder.SetPageSize(PageSize);

            var saleObjects = new List<SaleObject>();
            var pageNr = 0;

            SalesData salesData = null;

            do
            {
                try
                {
                    pageNr++;

                    urlBuilder.SetPageNr(pageNr);
                    salesData = await httpClient.GetFromJsonAsync<SalesData>(urlBuilder.AbsoluteUrl);

                    saleObjects.AddRange(salesData.SaleObjects);

                    progressCallback?.Invoke((pageNr * 100) / salesData.Paging.TotalPages);
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(5000);
                }
            } while ((pageNr < salesData.Paging.TotalPages));

            return saleObjects;
        }
    }
}
