using System;

namespace StatServerCore.ErrorHandling.Exceptions
{
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(DateTime timestamp)
            : base($"Match {timestamp} is not found.")
        {
        }
    }
}