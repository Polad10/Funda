using Funda.Enums;
using Funda.Services.Interfaces;

namespace Funda.Services
{
    public class FundaStat : IFundaStat
    {
        private IFundaApi _fundaApi;

        public FundaStat(IFundaApi fundaApi)
        {
            _fundaApi = fundaApi;
        }
        public async Task<Dictionary<string, int>> GetTopBrokers(string city, FundaObjectType type, bool withTuin, Action<int>? progressCallback = null, int top = 10)
        {
            var saleObjects = await _fundaApi.GetSaleObjects(city, type, withTuin, progressCallback);

            var brokerSaleObjects = saleObjects.GroupBy(s => s.BrokerName);
            var orderedBrokerSaleObjects = brokerSaleObjects.OrderByDescending(group => group.Count());

            return orderedBrokerSaleObjects.Take(top).ToDictionary(group => group.Key, group => group.Count());
        }
    }
}
