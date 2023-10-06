using Funda.Enums;
using Funda.Models;

namespace Funda.Services.Interfaces
{
    public interface IFundaApi
    {
        Task<List<SaleObject>> GetSaleObjects(string city, FundaObjectType type, bool withGarden,
            CancellationToken cancellationToken, Action<int>? progressCallback = null);
    }
}
