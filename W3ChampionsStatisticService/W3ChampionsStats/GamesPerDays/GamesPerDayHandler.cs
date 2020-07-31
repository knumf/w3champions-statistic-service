﻿using System;
using System.Threading.Tasks;
using W3ChampionsStatisticService.CommonValueObjects;
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

            var stat = await _w3Stats.LoadGamesPerDay(endTime, match.gameMode, match.gateway)
                       ?? GamesPerDay.Create(endTime, match.gameMode, match.gateway);
            var statOverallForGateway = await _w3Stats.LoadGamesPerDay(endTime, GameMode.Undefined, match.gateway)
                              ?? GamesPerDay.Create(endTime, GameMode.Undefined, match.gateway);

            var statForGameModeOnAllGateways = await _w3Stats.LoadGamesPerDay(endTime, match.gameMode, GateWay.Undefined)
                                          ?? GamesPerDay.Create(endTime, match.gameMode, GateWay.Undefined);
            var statOverall = await _w3Stats.LoadGamesPerDay(endTime, GameMode.Undefined, GateWay.Undefined)
                                          ?? GamesPerDay.Create(endTime, GameMode.Undefined, GateWay.Undefined);

            stat.AddGame();
            statOverall.AddGame();
            statOverallForGateway.AddGame();
            statForGameModeOnAllGateways.AddGame();

            await _w3Stats.Save(stat);
            await _w3Stats.Save(statOverall);
            await _w3Stats.Save(statOverallForGateway);
            await _w3Stats.Save(statForGameModeOnAllGateways);
        }
    }
}