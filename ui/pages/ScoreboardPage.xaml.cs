using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Memory.ui.pages
{
    public partial class ScoreboardPage : Page
    {
        public ScoreboardPage()
        {
            InitializeComponent();
            for (var i = 1; i < 11; i++)
            {
                var row = new TableRow();
                for (var j = 1; j < 4; j++)
                {
                    var cell = new TableCell();
                    cell.Blocks.Add(new Paragraph(new Run("Database top " + i)));
                    cell.Background = Brushes.Orange;
                    row.Cells.Add(cell);
                }

                table.Rows.Add(row);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().ChangePage(new MainPage());
        }
    }
}