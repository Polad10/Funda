using Funda.Constants;
using Funda.Enums;
using Funda.Exceptions;
using Funda.Helpers;
using Funda.Models;
using Funda.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Funda.Services
{
    public class FundaApi : IFundaApi
    {
        private const int PageSize = 25;
        private const int MaxRetries = 5;

        private HttpClient _httpClient;
        private int _delay;

        public FundaApi(IHttpClientFactory httpClientFactory)
        {
            _delay = 5000;
            _httpClient = httpClientFactory.CreateClient(FundaApiConstants.FundaHttpClientName);
        }

        public async Task<List<SaleObject>> GetSaleObjects(string city, FundaObjectType type, bool withGarden,
            CancellationToken cancellationToken, Action<int>? progressCallback = null)
        {
            var urlBuilder = CreateFundaApiUrlBuilder(city, type, withGarden);

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

        public void SetDelayBetweenRetries(int millisecondsDelay)
        {
            _delay = millisecondsDelay;
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
                    return await _httpClient.GetFromJsonAsync<SalesData>(url, cancellationToken);
                }
                catch (HttpRequestException)
                {
                    retryNr++;
                    await Task.Delay(_delay, cancellationToken);
                }
            } while (retryNr <= MaxRetries);

            throw new RetryLimitExceededException();
        }

        private FundaApiUrlBuilder CreateFundaApiUrlBuilder(string city, FundaObjectType type, bool withGarden)
        {
            var urlBuilder = new FundaApiUrlBuilder(_httpClient.BaseAddress);

            var searchKeys = new List<string>() { city };

            if (withGarden)
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
