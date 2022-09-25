using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JoshMkhariPROG7312Game.Logic.Home
{
    public class TextBlockModel
    {
        public static List<TextBlock> TextBlocksList;
        public static readonly string[] Difficulty = { "Easy", "Normal", "Difficult", "Hard", "Extreme", "Insane" };

        public static readonly int[][] WinData =
        {
            new[] { 0, 0 }, //Total Wins , // Total moves in wins
            new[] { 0, 0, 0, 0, 0, 0 }, //Number of wins within each difficulty
            new[] { 0, 0, 0, 0, 0, 0 } //Smallest amount of moves made to win within each difficulty
        };

        //Win Data
        //WinData 0 = Number of wins
        //WinData 1 = Smallest moves used
        public TextBlockModel()
        {
            TextBlocksList = new List<TextBlock>();
            CreateWinHistoryTextBlocks();
        }

        private static void CreateWinHistoryTextBlocks()
        {
            for (var i = 0; i < 6; i++)
            {
                var currentTextBlock = new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 15,
                    //https://stackoverflow.com/questions/5611658/change-margin-programmatically-in-wpf-c-sharp
                    Margin = new Thickness(10, 5, 0, 0),
                    Text = Difficulty[i] + ": " + "Wins: " + WinData[1][i] + " Best: " + WinData[2][i]
                };
                TextBlocksList.Add(currentTextBlock);
            }
        }
    }
}