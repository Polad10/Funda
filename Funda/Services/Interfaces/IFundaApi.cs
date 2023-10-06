using Funda.Enums;
using Funda.Models;

namespace Funda.Services.Interfaces
{
    public interface IFundaApi
    {
        Task<List<SaleObject>> GetSaleObjects(string city, FundaObjectType type, bool withTuin, Action<int>? progressCallback = null);
    }
}
