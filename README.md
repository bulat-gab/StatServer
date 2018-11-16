# StatServer

This is server for collecting statistics from online shooter games. Game servers send stats and StatServer aggregates and stores these data. It is built using ASP.NET Core WEB API 2 and MongoDb as a main data storage. This is study project.

# API 
/servers/info GET

/servers/(endpoint)/info PUT, GET

/servers/(endpoint)/matches/(timestamp) PUT, GET

/servers/(endpoint)/stats GET

/players/(name)/stats GET

/reports/recent-matches[/(count)] GET

/reports/best-players[/(count)] GET

/reports/popular-servers[/count] GET
  
  # Acknowledgements
  
  # License
  _Copyright &copy; 2018 Bulat Gabdrakhmanov - Provided under the [Apache 2.0 License](https://github.com/bulat-gab/StatServer/blob/master/LICENSE)_
