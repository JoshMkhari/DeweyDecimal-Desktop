using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using JoshMkhariPROG7312Game.ViewModels;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class ArrowModel
    {
        public int[][] ArrowDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        public List<Image> _directionArrowsList;
        public ArrowModel()
        {
            int[] leftValues = new []{ 407,407, 260,450 };
            int[] topValues = new []{ 139, 359,245,245 };
            
            ArrowDefaults = new int[2][];
            ArrowDefaults[0] = leftValues;
            ArrowDefaults[1] = topValues;
            
            _directionArrowsList = new List<Image>();
            CreateUpDownArrows();
        }

        public void DisableArrows(int arrowNum)//0,1,2,3
        {
            //Top section arrows are 0,1,2,3
            _directionArrowsList.ElementAt(arrowNum).Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
            //Bottom section arrows are 4 5 6 7
            _directionArrowsList.ElementAt(arrowNum+4).Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
        }

        public void UpdateArrowsEasyMode(ReplaceBooksViewModel replaceBooksViewModel)
        {
            for (int i = 0; i < 4; i++)
            {
                if (replaceBooksViewModel.CallNumberStacks.ElementAt(i).Count > 1)
                {
                    if (replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i*2) && replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i*2+1))
                    {
                        if (replaceBooksViewModel.CallNumberStacks.ElementAt(i).ElementAt(0) < replaceBooksViewModel.CallNumberStacks.ElementAt(i).ElementAt(1))
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'D'; //Store Descending for Left Rectangle
                            _directionArrowsList.ElementAt(i+4).Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));;
                        }
                        else
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'A'; //Store Ascending for Left Rectangle
                            _directionArrowsList.ElementAt(i).Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));;
                        }
                    }
                    else
                    {
                        if (replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i*2))
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'A'; //Store Ascending for Left Rectangle
                            _directionArrowsList.ElementAt(i).Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));;
                        }
                        else
                        {
                            replaceBooksViewModel.RectangleSortOrder[i] = 'D'; //Store Descending for Left Rectangle
                            _directionArrowsList.ElementAt(i+4).Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));;
                        }
                    }
                }
                else
                {
                    _directionArrowsList.ElementAt(i).Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                    _directionArrowsList.ElementAt(i+4).Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
                }
            }
        }
        private void CreateUpDownArrows()
        {
            //Creates top and bottom Up arrow sets
            for (int i = 0; i < 8; i++)
            {
                Image currentImage = new Image();
                currentImage.Width = 20;
                currentImage.Height = 20;
                if (i < 4)
                {
                    Canvas.SetLeft(currentImage,ArrowDefaults[0][i]);
                    Canvas.SetTop(currentImage,ArrowDefaults[1][i]);
                    currentImage.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                }
                else
                {
                    int current = i - 4;
                    Canvas.SetLeft(currentImage,ArrowDefaults[0][current]);
                    Canvas.SetTop(currentImage,ArrowDefaults[1][current]+45);
                    currentImage.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
                }
                _directionArrowsList.Add(currentImage);
            }
            
            //Creates top and bottom down arrow sets
        }
        
        public void UpdateArrows(ReplaceBooksViewModel replaceBooksViewModel)
        {
           
            _directionArrowsList.ElementAt(0).Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
            _directionArrowsList.ElementAt(1).Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));

            for (int i = 0; i < 4; i++)
            {
                _directionArrowsList.ElementAt(i).Source = replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i) ? new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative)) : new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
            }

            for (int i = 4; i < 8; i++)
            {
                _directionArrowsList.ElementAt(i).Source = replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(i) ? new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative)) : new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }
            
        }
    }
}