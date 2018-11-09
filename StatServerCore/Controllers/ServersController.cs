using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatServerCore.Model.DtoContracts;
using StatServerCore.Model.Mongo;

namespace StatServerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly IServersRepository serversRepository;

        public ServersController(IServersRepository serversRepository)
        {
            this.serversRepository = serversRepository;
        }

        /// <summary>
        ///     Information about all servers
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IEnumerable<Info>> Get()
        {
            return await serversRepository.GetAllServersInfo();
        }

        /// <summary>
        ///     Получение текущей информации об игровых серверах
        /// </summary>
        /// <remarks>
        ///     Если сервер с таким endpoint никогда не присылал advertise-запрос, нужно вернуть пустой ответ с кодом 404 Not
        ///     Found
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
        ///     Прием данных от сервера
        /// </summary>
        /// <param name="endpoint">уникальным идентификатором сервера</param>
        /// <param name="info">See this class: <see cref="Match" /></param>
        /// <returns>Последнюю версию информации, полученную PUT-запросом по этому адресу в том же формате</returns>
        [HttpPut("{endpoint}/info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<string>> ServerInfo(string endpoint, [FromBody] Info info)
        {
            await serversRepository.SaveServerInfo(endpoint, info);
            return Ok();
        }

        /// <summary>
        ///     Этот метод должен вернуть информацию о матче, полученную PUT-запросом по этому адресу в том же формате
        /// </summary>
        /// <remarks>Если PUT-запроса по этому адресу не было, нужно вернуть пустой ответ с кодом 404 Not Found</remarks>
        /// <param name="endpoint"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
        public ActionResult<Stats> Stats(string endpoint) => throw new NotImplementedException();
    }
}