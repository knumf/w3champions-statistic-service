namespace W3ChampionsStatisticService.W3ChampionsStats.MapsPerSeasons
{
    public class GamesPlayedOnMap
    {
        public static GamesPlayedOnMap Create(string map)
        {
            return new GamesPlayedOnMap
            {
                Map = map
            };
        }

        public string Map { get; set; }

        public void CountMatch()
        {
            Count++;
        }

        public int Count { get; set; }
    }
}