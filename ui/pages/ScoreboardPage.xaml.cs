using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Memory.ui.pages
{
    public partial class ScoreboardPage : Page
    {
        /// <summary>
        ///     This initialize the Components and start scoreboard generation
        /// </summary>
        public ScoreboardPage()
        {
            InitializeComponent();
            GenerateScoreboard();
        }

        /// <summary>
        ///     Generate scoreboard rows and cells
        /// </summary>
        private void GenerateScoreboard()
        {
            App.GetInstance().Database.Query("SELECT * FROM `scores` ORDER BY `score` DESC LIMIT 10;", reader =>
            {
                var index = 1;
                while (reader.Read())
                {
                    var row = new TableRow();
                    TableCell position, name, score;

                    position = new TableCell();
                    position.Blocks.Add(new Paragraph(new Run(Convert.ToString(index))));
                    position.Background = Brushes.Orange;
                    row.Cells.Add(position);

                    name = new TableCell();
                    name.Blocks.Add(new Paragraph(new Run(Convert.ToString(reader["name"]))));
                    name.Background = Brushes.Orange;
                    row.Cells.Add(name);

                    score = new TableCell();
                    score.Blocks.Add(new Paragraph(new Run(Convert.ToString(reader["score"]))));
                    score.Background = Brushes.Orange;
                    row.Cells.Add(score);

                    table.Rows.Add(row);
                    index++;
                }
            });
        }

        /// <summary>
        ///     Change page to Main Page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Back(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().ChangePage(new MainPage());
        }
    }
}