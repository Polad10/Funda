﻿using Funda.Enums;

namespace Funda.Services.Interfaces
{
    public interface IFundaStat
    {
        Task<Dictionary<string, int>> GetTopAgents(string city, FundaObjectType type, bool withGarden, CancellationToken cancellationToken,
            Action<int>? progressCallback = null, int top = 10);
    }
}
