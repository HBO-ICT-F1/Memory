using System;
using System.Diagnostics;
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
            catch (SqliteException e)
            {
                Assert.Fail("Failed to create SQLite database.");
            }
        }

        [Test]
        public void Can_Query_Database()
        {
            try
            {
                _sqLite.Query(
                    "CREATE TABLE IF NOT EXISTS `users`(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL);");
                _sqLite.Query("INSERT INTO `users`(name) VALUES('Robert');");
                _sqLite.Query("INSERT INTO `users`(name) VALUES('Kevin');");
                _sqLite.Query("INSERT INTO `users`(name) VALUES('Rutger');");

                _sqLite.Query("SELECT * FROM `users`;", reader =>
                {
                    Assert.False(reader.IsClosed);
                    Assert.True(reader.HasRows);
                    Assert.True(reader.NextResult());

                    // Try to get data from the database
                    var id = Convert.ToInt32(reader["id"]);
                    var name = Convert.ToInt32(reader["name"]);
                    Debug.WriteLine($"Found {name} at id {id}");
                    Assert.Pass("Successfully queried SQLite database.");
                });
            }
            catch (SqliteException)
            {
            }

            Assert.Fail("Failed to query SQLite database.");
        }
    }
}