using Microsoft.Data.Sqlite;

namespace Memory.utils
{
    /// <summary>
    ///     Util/wrapper for writing to and reading from an SQLite database
    /// </summary>
    public class SQLite
    {
        private readonly string _name;

        public SQLite(string name)
        {
            _name = name;
        }

        /// <summary>
        ///     Executes a query and returns an SQLiteDataReader
        /// </summary>
        /// <param name="query">The command to execute</param>
        /// <returns>The response from the database</returns>
        public SqliteDataReader Query(string query)
        {
            using (var connection = Connect())
            {
                var command = connection.CreateCommand();
                command.CommandText = query;
                return command.ExecuteReader();
            }
        }

        /// <summary>
        ///     Connects to the SQLite database
        /// </summary>
        /// <returns>The SQLiteConnection that was established</returns>
        private SqliteConnection Connect()
        {
            var connection = new SqliteConnection($"Data Source={_name}");
            connection.Open();
            return connection;
        }
    }
}