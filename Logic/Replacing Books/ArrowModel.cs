using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using JoshMkhariPROG7312Game.ViewModels;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class ArrowModel
    {
        public readonly List<Image> DirectionArrowsList;

        public ArrowModel()
        {
            int[] leftValues = { 407, 407, 260, 450 };
            int[] topValues = { 139, 359, 245, 245 };

            ArrowDefaults = new int[2][];
            ArrowDefaults[0] = leftValues;
            ArrowDefaults[1] = topValues;

            DirectionArrowsList = new List<Image>();
            CreateUpDownArrows();
        }

        private int[][] ArrowDefaults { get; } //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/

        public void DisableArrows(int arrowNum) //0,1,2,3
        {
            //Top section arrows are 0,1,2,3
            DirectionArrowsList.ElementAt(arrowNum).Source =
                new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
            //Bottom section arrows are 4 5 6 7
            DirectionArrowsList.ElementAt(arrowNum + 4).Source =
                new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
        }

        public void UpdateArrowsEasyMode(ReplaceBooksViewModel replaceBooksViewModel)
        {
            for (var i = 0; i < 4; i++)
                if (replaceBooksViewModel.CallNumberStacks.ElementAt(i).Count > 1)
                {
                    if (replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i) &&
                        replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i + 4))
                    {
                        if (replaceBooksViewModel.CallNumberStacks.ElementAt(i).ElementAt(0) <
                            replaceBooksViewModel.CallNumberStacks.ElementAt(i).ElementAt(1))
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'D'; //Store Descending for Left Rectangle
                            DirectionArrowsList.ElementAt(i + 4).Source =
                                new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));
                        }
                        else
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'A'; //Store Ascending for Left Rectangle
                            DirectionArrowsList.ElementAt(i).Source =
                                new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
                        }
                    }
                    else
                    {
                        if (replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i))
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'A'; //Store Ascending for Left Rectangle
                            DirectionArrowsList.ElementAt(i).Source =
                                new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
                        }
                        else
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'D'; //Store Descending for Left Rectangle
                            DirectionArrowsList.ElementAt(i + 4).Source =
                                new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));
                        }
                    }
                }
                else
                {
                    DirectionArrowsList.ElementAt(i).Source =
                        new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                    DirectionArrowsList.ElementAt(i + 4).Source =
                        new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
                }
        }

        private void CreateUpDownArrows()
        {
            //Creates top and bottom Up arrow sets
            for (var i = 0; i < 8; i++)
            {
                var currentImage = new Image
                {
                    Width = 20,
                    Height = 20
                };
                if (i < 4)
                {
                    Canvas.SetLeft(currentImage, ArrowDefaults[0][i]);
                    Canvas.SetTop(currentImage, ArrowDefaults[1][i]);
                    currentImage.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                }
                else
                {
                    var current = i - 4;
                    Canvas.SetLeft(currentImage, ArrowDefaults[0][current]);
                    Canvas.SetTop(currentImage, ArrowDefaults[1][current] + 45);
                    currentImage.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
                }

                DirectionArrowsList.Add(currentImage);
            }

            //Creates top and bottom down arrow sets
        }

        public void UpdateArrows(ReplaceBooksViewModel replaceBooksViewModel)
        {
            for (var i = 0; i < 4; i++)
                // DirectionArrowsList.ElementAt(i).Source = replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i) ? new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative)) : new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                if (replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i))
                {
                    DirectionArrowsList.ElementAt(i).Visibility = Visibility.Visible;
                    DirectionArrowsList.ElementAt(i).Source =
                        new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
                }
                else
                {
                    DirectionArrowsList.ElementAt(i).Visibility = Visibility.Collapsed;
                }

            for (var i = 4; i < 8; i++)
                if (replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i))
                {
                    DirectionArrowsList.ElementAt(i).Visibility = Visibility.Visible;
                    DirectionArrowsList.ElementAt(i).Source =
                        new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));
                }
                else
                {
                    DirectionArrowsList.ElementAt(i).Visibility = Visibility.Collapsed;
                }
        }
    }
}