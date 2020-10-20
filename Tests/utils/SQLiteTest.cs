using Memory.utils;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace Tests.utils
{
    /// <summary>
    ///     Class used for testing SQLite util
    /// </summary>
    [TestFixture]
    public class SQLiteTest
    {
        [SetUp]
        public void Init()
        {
            _sqLite = new SQLite("Test.db");
        }

        private SQLite _sqLite;

        [Test]
        public void Can_Query_Database()
        {
            try
            {
                _sqLite.Query("CREATE TABLE IF NOT EXISTS users(name text, age int)");
                Assert.Pass("Successfully queried SQLite database.");
            }
            catch (SqliteException e)
            {
                Assert.Fail("Failed to query SQLite database.");
            }
        }
    }
}