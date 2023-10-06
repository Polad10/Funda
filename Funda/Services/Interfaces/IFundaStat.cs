namespace Funda.Services.Interfaces
{
    public interface IFundaStat
    {
        Task<Dictionary<string, int>> GetTopBrokers(string city, bool withTuin, Action<int>? progressCallback = null, int top = 10);
    }
}
