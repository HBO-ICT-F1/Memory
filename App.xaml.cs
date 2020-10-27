using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Memory.utils;
using Microsoft.Data.Sqlite;

namespace Memory
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///     Memory app instance
        /// </summary>
        private static App _app;

        /// <summary>
        ///     The SQLite database where settings and scores are saved
        /// </summary>
        public readonly SQLite Database = new SQLite("memory.sql", SqliteOpenMode.ReadWriteCreate);

        /// <summary>
        ///     The media player used for audio
        /// </summary>
        public readonly MediaPlayer Player = new MediaPlayer();

        /// <summary>
        ///     The current game theme
        /// </summary>
        public string Theme = "dogs";

        public App()
        {
            // Set instance to this class
            _app = this;

            // Create table for saving data
            Database.Query(@"CREATE TABLE IF NOT EXISTS `settings` (
                name  VARCHAR(45) NOT NULL UNIQUE,
                value LONGTEXT NOT NULL
            );");

            // Create table for saving game
            Database.Query(@"CREATE TABLE IF NOT EXISTS `saves` (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                multiplayer TINYINT,
                game_size INT,
                players LONGTEXT,
                cards LONGTEXT,
                shown_cards LONGTEXT,
                theme VARCHAR(45)
            );");

            // Load media player volume
            var data = Database.Query("SELECT `value` FROM `settings` WHERE `name` = 'volume'");
            Player.Volume = 0.2;

            Player.Open(new Uri(
                $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{Theme}/default.mp3"));
            Player.Play();
        }

        /// <summary>
        ///     Gets the App instance
        /// </summary>
        /// <returns>The App instance that is currently running</returns>
        public static App GetInstance()
        {
            return _app;
        }
    }
}