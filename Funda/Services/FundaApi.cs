using Funda.Constants;
using Funda.Enums;
using Funda.Exceptions;
using Funda.Helpers;
using Funda.Models;
using Funda.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public async Task<List<SaleObject>> GetSaleObjects(string city, FundaObjectType type, bool withTuin,
            CancellationToken cancellationToken, Action<int>? progressCallback = null)
        {
            var urlBuilder = CreateFundaApiUrlBuilder(city, type, withTuin);

            var saleObjects = new List<SaleObject>();
            var pageNr = 1;

            SalesData salesData = null;

            do
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                urlBuilder.SetPageNr(pageNr);

                salesData = await GetSalesData(urlBuilder.AbsoluteUrl, cancellationToken);
                saleObjects.AddRange(salesData.SaleObjects);

                progressCallback?.Invoke(CalculateProgress(pageNr, salesData.Paging.TotalPages));
                pageNr++;
            } while ((pageNr <= salesData.Paging.TotalPages));

            return saleObjects;
        }

        private async Task<SalesData> GetSalesData(string url, CancellationToken cancellationToken)
        {
            int retryNr = 1;

            do
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                try
                {
                    return await _httpClient.GetFromJsonAsync<SalesData>(url);
                }
                catch (HttpRequestException)
                {
                    retryNr++;
                    await Task.Delay(5000);
                }
            } while (retryNr <= MaxRetries);

            throw new RetryLimitExceededException();
        }

        private FundaApiUrlBuilder CreateFundaApiUrlBuilder(string city, FundaObjectType type, bool withTuin)
        {
            var urlBuilder = new FundaApiUrlBuilder(_httpClient.BaseAddress);

            var searchKeys = new List<string>() { city };

            if (withTuin)
            {
                searchKeys.Add("tuin");
            }

            urlBuilder.SetType(type);
            urlBuilder.SetSearch(searchKeys);
            urlBuilder.SetPageSize(PageSize);

            return urlBuilder;
        }

        private int CalculateProgress(int current, int total)
        {
            return (current * 100) / total;
        }
    }
}
