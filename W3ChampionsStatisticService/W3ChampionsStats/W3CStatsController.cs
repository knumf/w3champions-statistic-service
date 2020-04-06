﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using W3ChampionsStatisticService.Ports;
using W3ChampionsStatisticService.W3ChampionsStats.DistinctPlayersPerDays;

namespace W3ChampionsStatisticService.W3ChampionsStats
{
    [ApiController]
    [Route("api/w3c-stats")]
    public class W3CStatsController : ControllerBase
    {
        private readonly IW3StatsRepo _w3StatsRepo;

        public W3CStatsController(IW3StatsRepo w3StatsRepo)
        {
            _w3StatsRepo = w3StatsRepo;
        }

        [HttpGet("map-race-wins")]
        public async Task<IActionResult> GetRaceVersusRaceStat()
        {
            var stats = await _w3StatsRepo.Load();
            return Ok(stats.StatsPerModes);
        }

        [HttpGet("games-per-day")]
        public async Task<IActionResult> GetGamesPerDay(DateTimeOffset from = default, DateTimeOffset to = default)
        {
            from = from != default ? from : DateTimeOffset.MinValue;
            to = to != default ? to : DateTimeOffset.MaxValue;
            var gameDays = await _w3StatsRepo.LoadGamesPerDayBetween(from, to);
            return Ok(gameDays);
        }

        [HttpGet("games-lengths")]
        public async Task<IActionResult> GetGameLengths()
        {
            var stats = await _w3StatsRepo.LoadGameLengths();
            return Ok(stats);
        }

        [HttpGet("distinct-players-per-day")]
        public async Task<IActionResult> DistinctPlayersPerDay(DateTimeOffset from = default, DateTimeOffset to = default)
        {
            from = from != default ? from : DateTimeOffset.MinValue;
            to = to != default ? to : DateTimeOffset.MaxValue;
            var stats = await _w3StatsRepo.LoadPlayersPerDayBetween(from, to);
            return Ok(stats.Select(s => new PlayersPerDayDto(s)));
        }
    }

    public class PlayersPerDayDto
    {
        public DateTimeOffset Date { get; }
        public long DistinctPlayers { get; }

        public PlayersPerDayDto(PlayersOnGameDay playersOnGameDay)
        {
            Date = playersOnGameDay.Date;
            DistinctPlayers = playersOnGameDay.DistinctPlayers;
        }
    }
}