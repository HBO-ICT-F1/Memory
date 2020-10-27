using System;
using Microsoft.Data.Sqlite;

namespace Memory.utils
{
    /// <summary>
    ///     Util/wrapper for writing to and reading from an SQLite database
    /// </summary>
    public class SQLite
    {
        private readonly SqliteOpenMode _mode;
        private readonly string _name;

        public SQLite(string name, SqliteOpenMode mode = SqliteOpenMode.ReadWriteCreate)
        {
            _name = name;
            _mode = mode;
        }

        /// <summary>
        ///     Executes a query and returns an SQLiteDataReader
        /// </summary>
        /// <param name="query">The command to execute</param>
        /// <param name="runnable">Used for handling the returned SqliteDataReader</param>
        /// <exception cref="SqliteException">An SQLite error occured during execution</exception>
        public void Query(string query, Action<SqliteDataReader> runnable = null)
        {
            using (var connection = Connect())
            {
                var command = connection.CreateCommand();
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    runnable?.Invoke(reader);
                }
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
                Mode = _mode
            };

            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            connection.Open();
            return connection;
        }
    }
}