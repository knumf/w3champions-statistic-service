﻿using System.Collections.Generic;
using System.Threading.Tasks;
using W3ChampionsStatisticService.MatchEvents;

namespace W3ChampionsStatisticService.Ports
{
    public interface IMatchEventRepository
    {
        Task<string> Insert(IList<MatchFinishedEvent> events);
        Task<IList<MatchFinishedEvent>> Load(string lastObjectId,  int pageSize = 100);
    }
}