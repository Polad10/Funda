using Funda.Models;
using Funda.Services.Interfaces;

namespace Funda.Services
{
    public class FundaApi : IFundaApi
    {
        public async Task<List<SaleObject>> GetSaleObjects(bool withTuin = false, Action<int>? progressCallback = null)
        {
            var baseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam/";

            if (withTuin)
            {
                baseUrl = $"{baseUrl}/tuin/";
            }

            var saleObjects = new List<SaleObject>();
            var pageNr = 0;


            using (var client = new HttpClient())
            {
                SalesData salesData = null;

                do
                {
                    try
                    {
                        pageNr++;
                        var url = $"{baseUrl}&page={pageNr}&pagesize=25";
                        salesData = await client.GetFromJsonAsync<SalesData>(url);

                        saleObjects.AddRange(salesData.SaleObjects);

                        progressCallback?.Invoke((pageNr * 100) / salesData.Paging.TotalPages);
                    }
                    catch (HttpRequestException)
                    {
                        await Task.Delay(5000);
                    }
                } while ((pageNr < salesData.Paging.TotalPages));
            }

            return saleObjects;
        }
    }
}
