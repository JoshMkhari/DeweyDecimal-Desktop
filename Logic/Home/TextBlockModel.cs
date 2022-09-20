using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JoshMkhariPROG7312Game.Logic.Home
{
    public class TextBlockModel
    {
        public static List<TextBlock>TextBlocksList;
        public static String[] difficulty = { "Easy", "Normal", "Difficult", "Hard", "Extreme", "Insane" };
        public static int[][] WinData = new int[][] //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        {
            new int[] {0, 0}, //Total Wins , // Total moves in wins
            new int[] {0, 0, 0, 0, 0, 0},//Number of wins within each difficulty
            new int[] {0, 0, 0, 0, 0, 0},//Smallest amount of moves made to win within each difficulty
        };
        public static bool Ran = false;
        
        //Win Data
            //WinData 0 = Number of wins
            //WinData 1 = Smallest moves used
        public TextBlockModel()
        {
            TextBlocksList = new List<TextBlock>();
            CreateWinHistoryTextBlocks();
        }

        private void CreateWinHistoryTextBlocks()
        {
            for (int i = 0; i < 6; i++)
            {
                TextBlock currentTextBlock = new TextBlock();
                currentTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                currentTextBlock.FontSize = 15;
                //https://stackoverflow.com/questions/5611658/change-margin-programmatically-in-wpf-c-sharp
                currentTextBlock.Margin = new Thickness(10, 5, 0, 0);
                currentTextBlock.Text = difficulty[i] + ": " + "Wins: " + WinData[1][i] + " Best: " + WinData[2][i];
                Debug.WriteLine("Current text " + currentTextBlock.Text);
                TextBlocksList.Add(currentTextBlock);
            }
        }
        
        
    }
}