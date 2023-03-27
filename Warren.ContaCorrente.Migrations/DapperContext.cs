using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace Warren.ContaCorrente.Migrations
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        => new MySqlConnection(_configuration.GetConnectionString("MYSQL_CONTACORRENTE"));
    }
}