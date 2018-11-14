using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using StatServerCore.Model.Mongo;

namespace StatServerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly IServersRepository serversRepository;

        public ServersController(IServersRepository serversRepository) => this.serversRepository = serversRepository;

        /// <summary>
        ///     Information about all servers
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IEnumerable<Info>> Get() => await serversRepository.GetAllServersInfo();

        /// <summary>
        ///     Information about particular server
        /// </summary>
        /// <remarks>
        ///     If there was no advertise-request with the same endpoint, 404 Not Found will be returned
        /// </remarks>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpGet("{endpoint}/info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Info>> ServerInfo(string endpoint)
        {
            var info = await serversRepository.GetServerInfo(endpoint);
            if (info == null)
            {
                return NotFound();
            }

            return info;
        }

        /// <summary>
        ///     Receiving data from game servers
        /// </summary>
        /// <param name="endpoint">Unique server identifier</param>
        /// <param name="info">See this class: <see cref="System.Text.RegularExpressions.Match" /></param>
        /// <returns>
        ///     The latest version of the information received by a PUT request at this address in the same format.
        /// </returns>
        [HttpPut("{endpoint}/info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<string>> ServerInfo(string endpoint, [FromBody] Info info)
        {
            await serversRepository.SaveServerInfo(endpoint, info);
            return Ok();
        }

        /// <summary>
        ///     Information about match, that has been received from PUT-request at this address.
        /// </summary>
        /// <remarks>
        ///     If there was no PUT request at this address, 404 Not Found will be returned.
        ///     Если PUT-запроса по этому адресу не было, нужно вернуть пустой ответ с кодом 404 Not Found
        /// </remarks>
        /// <param name="endpoint"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpGet("{endpoint}/matches/{timestamp}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Match> Match(string endpoint, DateTime timestamp) => throw new NotImplementedException();

        /// <summary>
        ///     Прием данных о сыгранном матче
        /// </summary>
        /// <remarks>
        ///     Результаты матчей от серверов, не приславших advertise-запрос, не должны сохраняться. Таким серверам нужно
        ///     отвечать пустым ответом с кодом 400 Bad Request
        /// </remarks>
        /// <param name="endpoint"></param>
        /// <param name="timestamp"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPut("{endpoint}/matches/{timestamp}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<string> Match(string endpoint, DateTime timestamp, [FromBody] Match match) => throw new NotImplementedException();

        /// <summary>
        ///     Receive server's stats
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>
        ///     <see cref="Stats" />
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{endpoint}/stats")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ServerStats> Stats(string endpoint) => throw new NotImplementedException();
    }
}