namespace Funda.Services.Interfaces
{
    public interface IFundaStat
    {
        Task<Dictionary<string, int>> GetTopBrokers(bool withTuin, Action<int>? progressCallback = null, int top = 10);
    }
}
