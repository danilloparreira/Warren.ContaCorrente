using Dapper;

namespace Warren.ContaCorrente.Migrations
{
    public class Database
    {
        private readonly DapperContext _context;

        public Database(DapperContext context)
        {
            _context = context;
        }

        public void CreateDatabase(string dbName)
        {
            using var connection = _context.CreateConnection();
            connection.Execute($"CREATE DATABASE {dbName}");
        }
    }
}
