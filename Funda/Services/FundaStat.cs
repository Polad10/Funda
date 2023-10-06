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
        public async Task<Dictionary<string, int>> GetTopAgents(string city, FundaObjectType type, bool withGarden,
            CancellationToken cancellationToken, Action<int>? progressCallback = null, int top = 10)
        {
            var saleObjects = await _fundaApi.GetSaleObjects(city, type, withGarden, cancellationToken, progressCallback);

            var agentSaleObjects = saleObjects.GroupBy(s => s.AgentName);
            var orderedAgentSaleObjects = agentSaleObjects.OrderByDescending(group => group.Count());

            return orderedAgentSaleObjects.Take(top).ToDictionary(group => group.Key, group => group.Count());
        }
    }
}
