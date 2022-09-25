using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using JoshMkhariPROG7312Game.ViewModels;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class BorderModel
    {
        public BorderModel()
        {
            BorderInitialLeft = 321;
            BorderDefaults = new byte[3][];
            CallBlockBordersList = new List<Border>();
            BorderInitialRight = new[] { 422, 198 };
            //https://www.rapidtables.com/convert/color/hex-to-rgb.html
            byte[] borderReds = { 108, 240, 176, 112, 254, 129, 39, 181, 30, 88 };
            byte[] borderGreen = { 71, 145, 198, 168, 202, 129, 51, 71, 136, 51 };
            byte[] borderBlues = { 34, 60, 83, 188, 80, 129, 73, 106, 109, 84 };

            BorderDefaults[0] = borderReds;
            BorderDefaults[1] = borderGreen;
            BorderDefaults[2] = borderBlues;
        }

        private double BorderInitialLeft { get; }
        private byte[][] BorderDefaults { get; } //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        private int[] BorderInitialRight { get; }
        public List<Border> CallBlockBordersList { get; }

        public void AssignValuesToBlocks(ReplaceBooksViewModel replaceBooksViewModel)
        {
            for (var i = 0; i < 10; i++)
            {
                var rectBlock = new Border
                {
                    //rectBlock.Name = "border" + i;
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    Width = 80,
                    Height = 22,
                    Background = new SolidColorBrush(Color.FromRgb(BorderDefaults[0][i], BorderDefaults[1][i],
                        BorderDefaults[2][i])) //https://www.rapidtables.com/convert/color/hex-to-rgb.html
                };

                var textForBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = NumberFormatter(replaceBooksViewModel.CallNumbers.ElementAt(i)) +
                           replaceBooksViewModel.CallNumbersStrings.ElementAt(i)
                };
                rectBlock.Child = textForBlock;
                CallBlockBordersList.Add(rectBlock);
            }

            PlaceBlocksAtStartPositions();
        }

        public void PlaceBlocksAtStartPositions()
        {
            for (var i = 0; i < 10; i++)
            {
                Canvas.SetLeft(CallBlockBordersList.ElementAt(i), BorderInitialLeft);
                if (i < 5)
                {
                    Canvas.SetTop(CallBlockBordersList.ElementAt(i), BorderInitialRight[0] - i * 27);
                }
                else
                {
                    var current = i - 5;
                    Canvas.SetTop(CallBlockBordersList.ElementAt(i), BorderInitialRight[1] - current * 27);
                }
            }
        }

        private static string NumberFormatter(double input)
        {
            var num = input.ToString(CultureInfo.InvariantCulture);

            //Look for comma
            var commaLocation = 0;
            for (var i = 0; i < num.Length; i++)
                if (num.Substring(i, 1).Equals(","))
                    //Comma found
                    commaLocation = i;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(num);
            while (commaLocation != 3)
            {
                stringBuilder.Insert(0,
                    "0"); //https://www.softwaretestinghelp.com/csharp-stringbuilder/#:~:text=C%23%20StringBuilder%20Methods%201%20%231%29%20Append%20Method%20As,Replace%20Method%20...%206%20%236%29%20Equals%20Method%20
                commaLocation++;
            }

            return stringBuilder.ToString();
        }
    }
}