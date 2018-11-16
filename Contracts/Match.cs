using System;

namespace Contracts
{
    public class Match
    {
        public string Map { get; set; }

        public GameMode GameMode { get; set; }

        public int FragLimit { get; set; }

        public int TimeLimit { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public Score[] Scoreboard { get; set; }
    }
}