using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Contracts.Exceptions;
using Newtonsoft.Json;

namespace Client
{
    public class StatServerClient : IStatServerClient
    {
        private static readonly JsonMediaTypeFormatter JsonFormatter = new JsonMediaTypeFormatter();
        private readonly HttpClient httpClient;

        public string BaseUrl { get; }

        public StatServerClient(string baseUrl)
        {
            BaseUrl = baseUrl;

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public StatServerClient() : this("https://localhost:5001/api/")
        {
        }

        public Task<Info[]> GetAllServersInfo()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "Servers/info");

            return RequestAsync<Info[]>(request);
        }

        public Task<Info> GetServerInfo(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Servers/{endpoint}/info");

            return RequestAsync<Info>(request);
        }

        public Task<string> SaveServerInfo(string endpoint, Info info)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"Servers/{endpoint}/info")
            {
                Content = new ObjectContent<Info>(info, JsonFormatter)
            };

            return RequestAsync<string>(request);
        }

        public Task<Match> GetMatch(string endpoint, DateTime timestamp)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Servers/{endpoint}/matches/{timestamp}");

            return RequestAsync<Match>(request);
        }

        public Task<string> SaveMatch(string endpoint, DateTime timestamp, Match match)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"Servers/{endpoint}/matches/{timestamp}")
            {
                Content = new ObjectContent<Match>(match, JsonFormatter)
            };

            return RequestAsync<string>(request);
        }

        public Task<ServerStats> GetServerStats(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Servers/{endpoint}/stats");

            return RequestAsync<ServerStats>(request);
        }

        public Task<PlayerStats> GetPlayersStats(string name) => throw new NotImplementedException();

        public Task<object> GetRecentMatches(int count = 5) => throw new NotImplementedException();

        public Task<object> GetBestPlayers(int count = 5) => throw new NotImplementedException();

        public Task<object> GetPopularServers(int count = 5) => throw new NotImplementedException();

        private async Task<T> RequestAsync<T>(HttpRequestMessage request)
        {
            request.Headers.Add("X-Request-Id", Guid.NewGuid().ToString());

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(body);
            }
            catch (HttpRequestException e)
            {
                var error = JsonConvert.DeserializeObject<Error>(body);

                if (error == null)
                {
                    throw;
                }

                switch (error.ErrorCode)
                {
                    case "ServerNotFound":
                        throw new ServerNotFoundException(error.Message);

                    case "MatchNotFoundException":
                        throw new MatchNotFoundException(error.Message);

                    default:
                        throw new HttpRequestException(error.ToString());
                }
            }
        }
    }

    internal class Error
    {
        public string ErrorCode { get; set; }

        public string Message { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionType { get; set; }

        public string StackTrace { get; set; }

        public Error InnerException { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            ToStringBuilder(sb);
            return sb.ToString();
        }

        private void ToStringBuilder(StringBuilder sb)
        {
            sb.AppendLine($"Message: {Message}");

            if (!string.IsNullOrEmpty(ExceptionMessage))
            {
                sb.AppendLine($"ExceptionMessage: {ExceptionMessage}");
            }

            if (!string.IsNullOrEmpty(ExceptionType))
            {
                sb.AppendLine($"ExceptionType: {ExceptionType}");
            }

            if (!string.IsNullOrEmpty(StackTrace))
            {
                sb.AppendLine($"StackTrace: {StackTrace}");
            }

            if (InnerException != null)
            {
                sb.AppendLine("InnerException:");
                InnerException.ToStringBuilder(sb);
            }
        }
    }
}