using System;

namespace StatServerCore.Model.DtoContracts
{
    public class Match
    {
        public string Map { get; set; }

        public GameMode GameMode { get; set; }

        public int FragLimit { get; set; }

        public int TimeLimit { get; set; }

        public DateTime TimeElapsed { get; set; }

        public Score[] Scoreboard { get; set; }
    }
}