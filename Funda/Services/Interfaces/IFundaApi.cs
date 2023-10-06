using Funda.Models;

namespace Funda.Services.Interfaces
{
    public interface IFundaApi
    {
        Task<List<SaleObject>> GetSaleObjects(string city, bool withTuin, Action<int>? progressCallback = null);
    }
}
