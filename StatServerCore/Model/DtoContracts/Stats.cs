namespace StatServerCore.Model.DtoContracts
{
    public class Stats
    {
        public long TotalMatchesPlayed { get; set; }

        public int MaximumMatchesPerDay { get; set; }

        public double AverageMatchesPerDay { get; set; }

        public int MaximumPopulation { get; set; }

        public double AveragePopulation { get; set; }

        public GameMode[] Top5GameModes { get; set; }

        public string[] Top5Maps { get; set; }
    }
}