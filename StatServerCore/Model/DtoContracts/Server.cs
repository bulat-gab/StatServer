using Contracts;

namespace StatServerCore.Model.DtoContracts
{
    public class Server
    {
        public string Endpoint { get; set; }
        public Info Info { get; set; }
    }
}