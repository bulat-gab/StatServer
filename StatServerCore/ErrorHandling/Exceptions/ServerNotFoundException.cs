using System;

namespace StatServerCore.ErrorHandling.Exceptions
{
    public class ServerNotFoundException : Exception
    {
        public ServerNotFoundException(string message) 
            : base($"Server {message} is not found.")
        {
        }
    }
}