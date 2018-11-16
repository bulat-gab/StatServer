using System;
using Contracts;

namespace StatServerCore.Extensions
{
    public static class Extentions
    {
        public static bool CompareWith(this DateTime dt1, DateTime dt2) => dt1.Second == dt2.Second &&
                                                                           dt1.Minute == dt2.Minute &&
                                                                           dt1.Day == dt2.Day &&
                                                                           dt1.Hour == dt2.Hour &&
                                                                           dt1.Month == dt2.Month &&
                                                                           dt1.Year == dt2.Year;

        public static bool IsEmpty(this ServerStats s)
        {
            return s.TotalMatchesPlayed == 0
                   && s.MaximumMatchesPerDay == 0
                   && s.AverageMatchesPerDay == 0
                   && s.MaximumPopulation == 0
                   && s.AveragePopulation == 0
                   && s.Top5GameModes.Equals(Array.Empty<GameMode>())
                   && s.Top5Maps.Equals(Array.Empty<string>());
        }
    }
}