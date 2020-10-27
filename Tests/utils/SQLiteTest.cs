using System;
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
            // Initialize database and drop table from previous runs
            _sqLite = new SQLite("Test.sql");
            _sqLite.Query("DROP TABLE `users`;");
        }

        private SQLite _sqLite;

        [Test]
        public void Can_Create_Database()
        {
            try
            {
                _sqLite.Query(
                    "CREATE TABLE IF NOT EXISTS `users`(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL);");
                Assert.Pass("Successfully created SQLite database.");
            }
            catch (SqliteException)
            {
                Assert.Fail("Failed to create SQLite database.");
            }
        }

        [Test]
        public void Can_Query_Database()
        {
            try
            {
                // Create database if Can_Create_Database test wasn't run
                _sqLite.Query(
                    "CREATE TABLE IF NOT EXISTS `users`(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL);");

                // Add test data
                _sqLite.Query("INSERT INTO `users`(name) VALUES('Robert');");
                _sqLite.Query("INSERT INTO `users`(name) VALUES('Kevin');");
                _sqLite.Query("INSERT INTO `users`(name) VALUES('Rutger');");

                _sqLite.Query("SELECT * FROM `users`;", (reader, _) =>
                {
                    // Go to next row, or fail if there is no data
                    Assert.True(reader.Read());

                    // Read data
                    var name = Convert.ToString(reader["name"]);
                    var id = Convert.ToInt32(reader["id"]);

                    // Mark as success
                    Assert.Pass($"Successfully queried SQLite database, found {name} at {id}");
                });
            }
            catch (SqliteException)
            {
            }

            Assert.Fail("Failed to query SQLite database.");
        }
    }
}