﻿using System;
using System.Threading.Tasks;
using W3ChampionsStatisticService.PadEvents;
using W3ChampionsStatisticService.Ports;
using W3ChampionsStatisticService.ReadModelBase;

namespace W3ChampionsStatisticService.W3ChampionsStats.GamesPerDays
{
    public class GamesPerDayHandler : IReadModelHandler
    {
        private readonly IW3StatsRepo _w3Stats;

        public GamesPerDayHandler(
            IW3StatsRepo w3Stats
            )
        {
            _w3Stats = w3Stats;
        }

        public async Task Update(MatchFinishedEvent nextEvent)
        {
            if (nextEvent.WasFakeEvent) return;
            var match = nextEvent.match;
            var endTime = DateTimeOffset.FromUnixTimeMilliseconds(match.endTime).Date;

            var stat = await _w3Stats.LoadGamesPerDay(endTime) ?? GamesPerDay.Create(endTime);

            stat.AddGame();

            await _w3Stats.Save(stat);
        }
    }
}