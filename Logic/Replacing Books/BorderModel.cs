using System;
using System.Collections.Generic;
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
        public double BorderInitialLeft { get;}
        public byte[][] BorderDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        public int[] BorderInitialRight { get; }
        public List<Border> _CallBlockBordersList { get; }

        public BorderModel()
        {
            BorderInitialLeft = 321;
            BorderDefaults = new byte[3][];
            _CallBlockBordersList = new List<Border>();
            BorderInitialRight = new []{ 422, 198 };
            //https://www.rapidtables.com/convert/color/hex-to-rgb.html
            byte[] borderReds = {108,240, 176,112,254,129,39,181,30,88};
            byte[] borderGreen = {71,145, 198,168,202,129,51,71,136,51};
            byte[] borderBlues= { 34,60,83,188,80,129,73,106,109,84};
            
            BorderDefaults[0] = borderReds;
            BorderDefaults[1] = borderGreen;
            BorderDefaults[2] = borderBlues;
        }
        
        public void AssignValuesToBlocks(ReplaceBooksViewModel _replaceBooksViewModel)
        {
            
            for (int i = 0; i < 10; i++)
            {
                
                Border rectBlock = new Border();
                //rectBlock.Name = "border" + i;
                rectBlock.BorderThickness = new Thickness(2);
                rectBlock.BorderBrush = new SolidColorBrush(Colors.Black);
                
                rectBlock.Width = 80;
                rectBlock.Height = 22;
                rectBlock.Background = new SolidColorBrush(Color.FromRgb(BorderDefaults[0][i], BorderDefaults[1][i], BorderDefaults[2][i]));//https://www.rapidtables.com/convert/color/hex-to-rgb.html
            
                TextBlock textForBlock = new TextBlock();
                textForBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textForBlock.Text = NumberFormatter(_replaceBooksViewModel.CallNumbers.ElementAt(0)) + 
                                    _replaceBooksViewModel.CallNumbersStrings.ElementAt(0);
                rectBlock.Child = textForBlock; 
                
                Canvas.SetLeft(rectBlock,BorderInitialLeft);
                if (i < 5)
                {
                    Canvas.SetTop(rectBlock,BorderInitialRight[0]-(i*27));  
                }
                else
                {
                    int current = i - 5;
                    Canvas.SetTop(rectBlock,BorderInitialRight[1]-(current*27));
                }
                _CallBlockBordersList.Add(rectBlock);
            }
            

        }
        
        private String NumberFormatter(double input)
        {
            String num = input.ToString();
            
            //Look for comma
            int commaLocation = 0;
            for (int i = 0; i < num.Length; i++)
            {
                if (num.Substring(i, 1).Equals(","))
                {
                    //Comma found
                    commaLocation = i;
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(num);
            while (commaLocation!=3)
            {
                stringBuilder.Insert(0, "0");//https://www.softwaretestinghelp.com/csharp-stringbuilder/#:~:text=C%23%20StringBuilder%20Methods%201%20%231%29%20Append%20Method%20As,Replace%20Method%20...%206%20%236%29%20Equals%20Method%20
                commaLocation++;
            }
            
            return stringBuilder.ToString();
        }
    }
}