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
        private static App app;

        /// <summary>
        ///     The SQLite database where settings and scores are saved
        /// </summary>
        public readonly SQLite database = new SQLite("memory.db", SqliteOpenMode.ReadWriteCreate);

        /// <summary>
        ///     The media player used for audio
        /// </summary>
        public readonly MediaPlayer player = new MediaPlayer();

        /// <summary>
        ///     The current game theme
        /// </summary>
        public string theme = "dogs";

        public App()
        {
            // Set instance to this class
            app = this;

            // Create table for saving data
            database.Query("CREATE TABLE IF NOT EXISTS settings(name TEXT NOT NULL UNIQUE, value TEXT)");

            // Load media player volume
            var data = database.Query("SELECT `value` FROM `settings` WHERE `name` = 'volume'");
            player.Volume = 0.2;

            player.Open(new Uri(
                $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{theme}/default.mp3"));
            player.Play();
        }

        /// <summary>
        ///     Gets the App instance
        /// </summary>
        /// <returns>The App instance that is currently running</returns>
        public static App GetInstance()
        {
            return app;
        }
    }
}