using Microsoft.Data.Sqlite;

namespace Memory.utils
{
    /// <summary>
    ///     Util/wrapper for writing to and reading from an SQLite database
    /// </summary>
    public class SQLite
    {
        private readonly SqliteOpenMode _mode;
        private readonly string _name, _password;

        public SQLite(string name) : this(name, SqliteOpenMode.ReadWriteCreate, string.Empty)
        {
        }

        public SQLite(string name, SqliteOpenMode mode) : this(name, mode, string.Empty)
        {
        }

        public SQLite(string name, SqliteOpenMode mode, string password)
        {
            _name = name;
            _mode = mode;
            _password = password;
        }

        /// <summary>
        ///     Executes a query and returns an SQLiteDataReader
        /// </summary>
        /// <param name="query">The command to execute</param>
        /// <returns>The response from the database</returns>
        /// <exception cref="SqliteException">An SQLite error occured during execution</exception>
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
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = _name,
                Mode = _mode,
                Password = _password
            };

            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            connection.Open();
            return connection;
        }
    }
}