using System;

namespace Contracts
{
    public class ServerStats
    {
        public long TotalMatchesPlayed { get; set; }

        public int MaximumMatchesPerDay { get; set; }

        public double AverageMatchesPerDay { get; set; }

        public int MaximumPopulation { get; set; }

        public double AveragePopulation { get; set; }

        public GameMode[] Top5GameModes { get; set; }

        public string[] Top5Maps { get; set; }

        public static ServerStats CreateEmpty()
        {
            return new ServerStats
            {
                TotalMatchesPlayed = 0,
                MaximumMatchesPerDay = 0,
                AverageMatchesPerDay = 0,
                MaximumPopulation = 0,
                AveragePopulation = 0,
                Top5GameModes = Array.Empty<GameMode>(),
                Top5Maps = Array.Empty<string>()
            }; 
        }
    }
}