using Funda.Models;

namespace Funda.Services.Interfaces
{
    public interface IFundaApi
    {
        Task<List<SaleObject>> GetSaleObjects(bool withTuin = false, Action<int>? progressCallback = null);
    }
}
