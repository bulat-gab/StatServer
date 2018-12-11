using System;

namespace Contracts.Exceptions
{
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(string timestamp)
            : base($"Match {timestamp} is not found.")
        {
        }
    }
}