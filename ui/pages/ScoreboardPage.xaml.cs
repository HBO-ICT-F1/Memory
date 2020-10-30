using System.Windows.Controls;
using System.Collections.Generic;
using System;
using System.Windows.Documents;

namespace Memory.ui.pages
{
    public partial class ScoreboardPage : Page
    {
        public ScoreboardPage()
        {
            InitializeComponent();
            var scores = new List<Tuple<int, string, int>>();
            scores.Add(new Tuple<int, string, int>(1, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(2, "Robert", 14));
            scores.Add(new Tuple<int, string, int>(3, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(4, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(5, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(6, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(1, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(7, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(8, "Jesse", 15));
            scores.Add(new Tuple<int, string, int>(9, "Jesse", 15));

            foreach (var item in scores)
            {
                var group = new TableRowGroup();
                var row = new TableRow();
                var positionCell = AddCell(item.Item1);
                var nameCell = AddCell(item.Item2);
                var scoreCell = AddCell(item.Item3);                
            }
        }

        private TableCell AddCell(object content)
        {
            var cell = new TableCell();
            var run = new Run(content.ToString());
            var paragraph = new Paragraph(run);
            cell.Blocks.Add(paragraph);
            return cell;
        }
    }   
}