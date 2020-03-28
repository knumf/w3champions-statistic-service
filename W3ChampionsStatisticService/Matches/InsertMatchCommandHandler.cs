﻿using System.Linq;
using System.Threading.Tasks;
using W3ChampionsStatisticService.Ports;

namespace W3ChampionsStatisticService.Matches
{
    public class InsertMatchCommandHandler
    {
        private readonly IMatchEventRepository _eventRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IVersionRepository _versionRepository;

        public InsertMatchCommandHandler(
            IMatchEventRepository eventRepository,
            IMatchRepository matchRepository,
            IVersionRepository versionRepository)
        {
            _eventRepository = eventRepository;
            _matchRepository = matchRepository;
            _versionRepository = versionRepository;
        }

        public async Task Update()
        {
            var lastVersion = await _versionRepository.GetLastVersion<InsertMatchCommandHandler>();
            var nextEvents = await _eventRepository.Load(lastVersion);

            var matchups = nextEvents.Select(e => new Matchup(e)).ToList();

            await _matchRepository.Upsert(matchups);
            await _versionRepository.SaveLastVersion<InsertMatchCommandHandler>("tbd");
        }
    }
}