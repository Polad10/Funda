﻿using Funda.Enums;

namespace Funda.Services.Interfaces
{
    public interface IFundaStat
    {
        Task<Dictionary<string, int>> GetTopBrokers(string city, FundaObjectType type, bool withTuin, Action<int>? progressCallback = null, int top = 10);
    }
}
