﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;

namespace JoshMkhariPROG7312Game.Views
{
    /// <summary>
    ///     Interaction logic for ReplaceBooksView.xaml
    /// </summary>
    public partial class ReplaceBooksView : UserControl
    {
        private int _activatedBlockCount;

        private SolidColorBrush _blackBrush;
        private readonly int[] _bottomRectCanvasYLocations = { 422, 396, 369, 342, 315, 288 }; //For bottom rectangle
        private readonly int[] _topRectCanvasYLocations = { 198, 171, 144, 117, 90, 63}; //For top rectangle
        
        private int _currentDifficulty; //0 for easy, 5 for insane

        private Stack<double> _callNumbers, _callNumbersTop, _callNumbersBottom, _callNumbersLeft, _callNumbersRight; //To store initial numbers in top rectangle
        private Stack<String> _callNumbersStrings;
        private int _destinationRectangleNumber; //The rectangle a block is being transferred to
        private PreSetDifficulty _preSetDifficulty = new PreSetDifficulty();
        private bool _onSettingsPage = false;
        
        private int _originalRectangleNumber; //??????????????????????????????????????

        private readonly char[] _rectangleSortOrder = new char [4]; //Store whether rectangle is ascending or descending
        private double[] _stackSizes = new double[4];
        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        private readonly IDictionary<double, int>
            _rectValueNamePair = new Dictionary<double, int>(); //Stores Random value and Rectangle name
        
        private IDictionary<int, bool>
            _activeAscDesc = new Dictionary<int, bool>(); //Stores Set difficulty for current game

        //Store locations where blocks must rest based on amount of items within rectangle
        

        private int _movesCount;
        public ReplaceBooksView()
        {
            InitializeComponent();
            _currentDifficulty = 0;
            _activeAscDesc = _preSetDifficulty.ChangeDifficulty(_currentDifficulty);
            _originalRectangleNumber = 4;
            _activatedBlockCount = 0;
            _rectangleSortOrder[0] = 'A';
            _rectangleSortOrder[1] = 'A';
            _movesCount = 0;

            var blackBrush = new SolidColorBrush(Colors.Blue);
            RegisterName("MySolidColorBorderBrush", blackBrush);
            
            _callNumbers = new Stack<double>(); //To store initial 10 values
            _callNumbersStrings = new Stack<string>(); //To store initial chars
            //To generate random numbers https://www.tutorialsteacher.com/articles/generate-random-numbers-in-csharp
            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                _callNumbers.Push((rnd.Next(1,((99999)*10+1))+1*10)/1000.0); //add a value
                _callNumbersStrings.Push(" " +(char) rnd.Next(65, 90) + (char) rnd.Next(65, 90) + (char) rnd.Next(65, 90));
                //https://stackoverflow.com/questions/27531759/generating-decimal-random-numbers-in-java-in-a-specific-range
            }
            _callNumbersTop = new Stack<double>();
            _callNumbersBottom = new Stack<double>();
            _callNumbersLeft = new Stack<double>();
            _callNumbersRight = new Stack<double>();

            InitializeStacks();
            
            AssignValuesToBlocks();
        }

        private void InitializeStacks()
        {
            for (var i = 0; i < 5; i++) _callNumbersTop.Push(_callNumbers.ElementAt(i));

            for (var i = 5; i < 10; i++) _callNumbersBottom.Push(_callNumbers.ElementAt(i));
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
        
        private void AssignValuesToBlocks()
        {
            txtRectBlock1.Text = NumberFormatter(_callNumbersTop.ElementAt(4)) +_callNumbersStrings.ElementAt(0);
            txtRectBlock2.Text = NumberFormatter(_callNumbersTop.ElementAt(3))+ _callNumbersStrings.ElementAt(1);
            txtRectBlock3.Text =  NumberFormatter(_callNumbersTop.ElementAt(2))+ _callNumbersStrings.ElementAt(2);
            txtRectBlock4.Text =  NumberFormatter(_callNumbersTop.ElementAt(1))+ _callNumbersStrings.ElementAt(3);
            txtRectBlock5.Text =  NumberFormatter(_callNumbersTop.ElementAt(0))+_callNumbersStrings.ElementAt(4);
            
            //https://www.tutorialsteacher.com/csharp/csharp-dictionary
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(4), 1); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(3), 2); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(2), 3); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(1), 4); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(0), 5); //storing the value with the rectangle name
            
            txtRectBlock6.Text =  NumberFormatter(_callNumbersBottom.ElementAt(0))+_callNumbersStrings.ElementAt(5);
            txtRectBlock7.Text =  NumberFormatter(_callNumbersBottom.ElementAt(1))+ _callNumbersStrings.ElementAt(6);
            txtRectBlock8.Text =  NumberFormatter(_callNumbersBottom.ElementAt(2))+ _callNumbersStrings.ElementAt(7);
            txtRectBlock9.Text =  NumberFormatter(_callNumbersBottom.ElementAt(3))+ _callNumbersStrings.ElementAt(8);
            txtRectBlock10.Text =  NumberFormatter(_callNumbersBottom.ElementAt(4))+_callNumbersStrings.ElementAt(9);

            //https://www.tutorialsteacher.com/csharp/csharp-dictionary
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(0), 6); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(1), 7); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(2), 8); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(3), 9); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(4), 10); //storing the value with the rectangle name

            
            //Set initial Difficulty
            
        }
        //To colour block strokes
        private void ActivateBlockColour(Rectangle rect, int mode)
        {
            if (_callNumbersLeft.Count < 1)
            {
                imgLeftRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }            
            if (_callNumbersRight.Count < 1)
            {
                imgRightRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                imgRightRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }
            
            switch (mode)
            {
                case 0:
                    _blackBrush = new SolidColorBrush(Colors.Gold); //Sender
                    _activatedBlockCount++; //Program now knows start, waiting for destination
                    break;
                case 1:
                    _blackBrush = new SolidColorBrush(Colors.Blue); //Reciever
                    break;
                case 2:
                    _blackBrush = new SolidColorBrush(Colors.Red); //Reciever
                    break;
                default:
                    _blackBrush = new SolidColorBrush(Colors.Transparent); //Error
                    break;
            }

            rect.StrokeThickness = 3;
            rect.Stroke = _blackBrush;
        }

        private void ReplacingBooks_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        //To make a block show red when an error occurs
        private void ActivateRedError(Rectangle rect, string errorText)
        {
            ActivateBlockColour(rect, 2); //Display Red
            MessageBox.Show("Action not allowed " + errorText);
            ClearAllFocus();
            _activatedBlockCount = 0;
        }

        //To clear all colours surrounding blocks
        private void ClearAllFocus()
        {
            ActivateBlockColour(selectTopRect, 3); //Make Transparent
            ActivateBlockColour(selectBottomRect, 3); //Make Transparent
            ActivateBlockColour(selectLeftRect, 3); //Make Transparent
            ActivateBlockColour(selectRightRect, 3); //Make Transparent
            _activatedBlockCount = 0;
            _destinationRectangleNumber = 0;
        }

        //To push a block number from one stack to another
        private void UpdateStack(int rectangleNumber)
        {
            AnimateBlockMovement();
            _movesCount++;
            txtMovesCount.Content = "Moves: " + _movesCount;
            switch (rectangleNumber)
            {
                case 0:
                    _callNumbersTop.Push(PopCallBlock(_originalRectangleNumber)); //Push to top stack
                    break;
                case 1:
                    _callNumbersBottom.Push(PopCallBlock(_originalRectangleNumber)); //Push to bottom stack
                    break;
                case 2:
                    _callNumbersLeft.Push(PopCallBlock(_originalRectangleNumber)); //Push to left stack
                    break;
                case 3:
                    _callNumbersRight.Push(PopCallBlock(_originalRectangleNumber)); //Push to right stack
                    break;
            }

            //RULES PLACED HERE
            if (_callNumbersLeft.Count > 1)
            {
                Debug.WriteLine("This is element 4 " + _activeAscDesc.Values.ElementAt(4));
                Debug.WriteLine("This is element 4 " + _activeAscDesc.Values.ElementAt(5));
                if (_activeAscDesc.Values.ElementAt(4) && _activeAscDesc.Values.ElementAt(5))
                {
                    if (_callNumbersLeft.ElementAt(0) < _callNumbersLeft.ElementAt(1))
                    {
                        _rectangleSortOrder[2] = 'D'; //Store Descending for Left Rectangle
                        imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));;
                    }
                    else
                    {
                        _rectangleSortOrder[2] = 'A'; //Store Ascending for Left Rectangle
                        imgLeftRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));;
                    }
                }
                else
                {
                    if (_activeAscDesc.Values.ElementAt(4) )
                    {
                        _rectangleSortOrder[2] = 'A'; //Store Ascending for Left Rectangle
                        imgLeftRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));;
                    }
                    else
                    {
                        _rectangleSortOrder[2] = 'D'; //Store Descending for Left Rectangle
                        imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));;
                    }
                }
            }
            else
            {
                imgLeftRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
                imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }

            if (_callNumbersRight.Count > 1)
            {
                Debug.WriteLine("This is element 6 " + _activeAscDesc.Values.ElementAt(6));
                Debug.WriteLine("This is element 7 " + _activeAscDesc.Values.ElementAt(7));
                if (_activeAscDesc.Values.ElementAt(6) && _activeAscDesc.Values.ElementAt(7))
                {
                    if (_callNumbersRight.ElementAt(0) < _callNumbersRight.ElementAt(1))
                    {
                        _rectangleSortOrder[3] = 'D'; //Store Descending for Right Rectangle
                        imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));;
                    }
                    else
                    {
                        _rectangleSortOrder[3] = 'A'; //Store Ascending for Right Rectangle
                        imgRightRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));;
                    }
                }
                else
                {
                    if (_activeAscDesc.Values.ElementAt(6))
                    {
                        _rectangleSortOrder[3] = 'A'; //Store Ascending for Right Rectangle
                        imgRightRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));;
                    }
                    else
                    {
                        _rectangleSortOrder[3] = 'D'; //Store Descending for Right Rectangle
                        imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));;
                    }
                }
            }
            else
            {
                imgRightRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));;
                imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));;
            }

            txtTopRectStorageCapacity.Content = (_callNumbersTop.Count / _stackSizes[0])*100 + "%";
            txtBottomRectStorageCapacity.Content = (_callNumbersBottom.Count / _stackSizes[1])*100 + "%";
            txtLeftRectStorageCapacity.Content = (_callNumbersLeft.Count / _stackSizes[2])*100 + "%";
            txtRightRectStorageCapacity.Content = (_callNumbersRight.Count / _stackSizes[3])*100 + "%";
        }

        private void SelectedRectangle(Rectangle currentRectangle, Stack<double> currentRectangleStack,
            int currentRectangleNumber)
        {
            var isEmptyRect = !currentRectangleStack.Any(); //check if the list is empty
            if (_activatedBlockCount == 0) //This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect) //No more blocks to move
                {
                    ActivateRedError(currentRectangle, "This rectangle is empty, cannot send anything from it");
                }
                else
                {
                    _originalRectangleNumber = currentRectangleNumber; //Represents 
                    ActivateBlockColour(currentRectangle, 0); //Display Gold as this is start block
                }
            }
            else //this is destination block
            {
                //check num of blocks within rect
                if (isEmptyRect) //can add block
                {
                    if (_originalRectangleNumber != currentRectangleNumber) //Make sure the source and the destination are not the same
                    {
                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                        _destinationRectangleNumber = currentRectangleNumber; //Set destination 
                        UpdateStack(currentRectangleNumber);
                    }
                    else
                    {
                        ActivateRedError(currentRectangle, "You cannot send a call number to the same start");
                    }
                }
                else
                {
                    if (_originalRectangleNumber == currentRectangleNumber)
                    {
                        ActivateRedError(currentRectangle, "You cannot send a call number to the same start");
                    }
                    else
                    { 
                        if (currentRectangleStack.Count < _stackSizes[currentRectangleNumber]) //If there is still space for the block
                        {
                            if (currentRectangleStack.Count == 1 && _activeAscDesc.Values.ElementAt(currentRectangleNumber*2) &&_activeAscDesc.Values.ElementAt(currentRectangleNumber*2+1)) //If there is only one block inside and no rules on block
                            {
                                ActivateBlockColour(currentRectangle, 1); //Make Blue
                                _destinationRectangleNumber = currentRectangleNumber; //Set destination 
                                UpdateStack(currentRectangleNumber);
                            }
                            else
                            {
                                //What is my order
                                if (_rectangleSortOrder[currentRectangleNumber] == 'A' || _activeAscDesc.Values.ElementAt(currentRectangleNumber*2)) //If ascending
                                {
                                    if (PeepCallBlock(_originalRectangleNumber) >
                                        PeepCallBlock(currentRectangleNumber)) //If new addition is greater than top num
                                    {
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _destinationRectangleNumber = currentRectangleNumber; //Set destination 
                                        UpdateStack(currentRectangleNumber);
                                    }
                                    else
                                    {
                                        ActivateRedError(currentRectangle, "This is a ascending list");
                                    }
                                }
                                else if (_rectangleSortOrder[currentRectangleNumber] == 'D'||  _activeAscDesc.Values.ElementAt(currentRectangleNumber*2+1)) //If Descending
                                {
                                    if (PeepCallBlock(_originalRectangleNumber) <
                                        PeepCallBlock(currentRectangleNumber)) //If new addition is greater than top num
                                    {
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _destinationRectangleNumber = currentRectangleNumber; //Set destination 
                                        UpdateStack(currentRectangleNumber);
                                    }
                                    else
                                    {
                                        ActivateRedError(currentRectangle, "This is a descending list");
                                    }
                                }

                                //Does it match my order?
                            }
                        }
                        else
                        {
                            ActivateRedError(currentRectangle, "Full");
                        }   
                    }

                }
            }
        }

        private void selectTopRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectTopRect, _callNumbersTop, 0);
        }

        private void selectBottomRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectBottomRect, _callNumbersBottom, 1);
        }

        private void selectLeftRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectLeftRect, _callNumbersLeft, 2);
        }

        private void selectRightRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectRightRect, _callNumbersRight, 3);
        }

        //For both left and right rectangles
        private int ReturnCurrentBlockYLeftRight(Stack<double> rectStack)
        {
            switch (rectStack.Count + 1)
            {
                case 1: //Rectangle at bottom of rect list
                    return 320;
                case 2: //Second rectangle
                    return 293;
                case 3:
                    return 266;
                case 4:
                    return 239;
                case 5:
                    return 212;
                default: //Last rectangle
                    return 185;
            }
        }


        private void MoveRectangle(int blockNumber, int originRectangle)
        {
            switch (blockNumber)
            {
                case 1:
                    StartYJourney(rectBlock1, originRectangle);
                    break;
                case 2:
                    StartYJourney(rectBlock2, originRectangle);
                    break;
                case 3:
                    StartYJourney(rectBlock3, originRectangle);
                    break;
                case 4:
                    StartYJourney(rectBlock4, originRectangle);
                    break;
                case 5:
                    StartYJourney(rectBlock5, originRectangle);
                    break;
                case 6:
                    StartYJourney(rectBlock6, originRectangle);
                    break;
                case 7:
                    StartYJourney(rectBlock7, originRectangle);
                    break;
                case 8:
                    StartYJourney(rectBlock8, originRectangle);
                    break;
                case 9:
                    StartYJourney(rectBlock9, originRectangle);
                    break;
                case 10:
                    StartYJourney(rectBlock10, originRectangle);
                    break;
            }
        }

        private void StartXJourneyRight(Border border, int stop)
        {
            do
            {
                Canvas.SetLeft(border, Canvas.GetLeft(border) + 1);
            } while (Canvas.GetLeft(border) < stop);
        }

        private void StartXJourneyLeft(Border border, int stop)
        {
            do
            {
                Canvas.SetLeft(border, Canvas.GetLeft(border) - 1);
            } while (Canvas.GetLeft(border) > stop);
        }

        private void StartYJourneyUp(Border border)
        {
            var destinationY = 0;
            switch (_destinationRectangleNumber)
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersTop, _topRectCanvasYLocations);
                    break;
                case 1: // For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersBottom, _bottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    break;
                case 3: // For right block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    break;
            }
            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) + 1);
            } while (Canvas.GetTop(border) < destinationY); //285<320
            if (Canvas.GetTop(border) > destinationY) //320 <320  285< 218
                do
                {
                    Canvas.SetTop(border, Canvas.GetTop(border) - 1);
                } while (Canvas.GetTop(border) > destinationY); //285>320  285>218
            //Check destination amount of elements
        }

        private void StartYJourneyDown(Border border)
        {
            var destinationY = 0;
            switch (_destinationRectangleNumber)
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersTop, _topRectCanvasYLocations);
                    break;
                case 1: // For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersBottom, _bottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    break;
                case 3: // For right block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    break;
            }
            
            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) + 1);
            } while (Canvas.GetTop(border) < destinationY);
            //Check destination amount of elements
        }

        //https://stackoverflow.com/questions/10298216/moving-any-control-in-wpf
        private void StartYJourney(Border border, int top)
        {
            //Move up based on current location to top of current rectangle
            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) - 1);
            } while (Canvas.GetTop(border) > top);

            //Determine if need to be going left or right
            
            switch (_destinationRectangleNumber) //0 Top, 1 Bottom, 2 Left, 3 Right
            {
                case 0: //Going to top
                    switch (_originalRectangleNumber)
                    {
                        case 1: //Starting from bottom
                            StartYJourneyUp(border); //Just go straight up
                            break;
                        case 2: //Starting from left
                            
                            StartXJourneyRight(border, 321);
                            //Then go right until reach 321
                            StartYJourneyUp(border); //Just go straight up
                            break;
                        case 3: //Starting from right
                            StartXJourneyLeft(border, 321);
                            //Then go left until reach 321
                            StartYJourneyUp(border); //Just go straight up
                            break;
                    }
                    break;
                case 1: //Going to Bottom 285
                    switch (_originalRectangleNumber)
                    {
                        case 0: //Starting from Top
                            //Just go straight down
                            StartYJourneyDown(border);
                            break;
                        case 2: //Starting from left
                            //Go right until reach 321
                            StartXJourneyRight(border, 321);
                            //Then go down until reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 3: //Starting from right
                            //Go left until reach 321
                            StartXJourneyLeft(border, 321);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }

                    break;
                case 2: //Going to Left 180
                    switch (_originalRectangleNumber)
                    {
                        case 0: //Starting from top
                            //Go left until 175
                            StartXJourneyLeft(border, 175);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 1: //Starting from bottom
                            StartXJourneyLeft(border, 175);
                            //Then go down reach saved location
                            StartYJourneyUp(border);
                            break;
                        case 3: //Starting from right
                            StartXJourneyLeft(border, 175);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }

                    break;
                case 3: //Going to Right one
                    switch (_originalRectangleNumber)
                    {
                        //478
                        case 0: //Starting from top
                            //Go right until 478
                            StartXJourneyRight(border, 478);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 1: //Starting from bottom
                            //Go up until top of right and left
                            //Go right until 478
                            StartXJourneyRight(border, 478);
                            //Then go down reach saved location
                            StartYJourneyUp(border);
                            break;
                        case 2: //Starting from left
                            //Go right until 478
                            StartXJourneyRight(border, 478);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }

                    break;
            }
        }

        private int ReturnCurrentBlockYTopBottom(Stack<double> rectStack, int[] locations)
        {
            int location; //To store where to place the block
            if (rectStack.Any())//If there are elements within the stack
            {
                location = rectStack.Count;
            }
            else
                location = 0;
            return locations[location];
        }

        private void AnimateBlockMovement()
        {
            switch (_originalRectangleNumber) //Based on original rectangle number between 0-3 
            {
                case 0: //From Top
                    for (var i = 0; i < _rectValueNamePair.Count; i++)
                        if (_rectValueNamePair.Keys.ElementAt(i) == _callNumbersTop.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i), 60);
                            break;
                        }

                    //canvasLeft = 321; //Location on x axis where block must stop within top and bottom rectangles
                    break;

                case 1: //From bottom rectangle
                    //canvasLeft = 321;//Location on x axis where block must stop within top and bottom rectangles
                    for (var i = 0; i < _rectValueNamePair.Count; i++)
                        if (_rectValueNamePair.Keys.ElementAt(i) == _callNumbersBottom.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i), 285);
                            break;
                        }

                    break;

                case 2: //From Left rectangle
                    //canvasTop = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    //return _callNumbersLeft.Pop();
                    for (var i = 0; i < _rectValueNamePair.Count; i++)
                        if (_rectValueNamePair.Keys.ElementAt(i) == _callNumbersLeft.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i), 180);
                            break;
                        }

                    //canvasLeft = 175;//Location on x axis where block must stop within left rectangle 
                    break;
                default://From Right rectangle
                    for (var i = 0; i < _rectValueNamePair.Count; i++)
                        if (_rectValueNamePair.Keys.ElementAt(i) == _callNumbersRight.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i), 180);
                            break;
                        }

                    //For right rectangle
                    //canvasTop = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    //canvasLeft = 478;//Location on x axis where block must stop within right rectangle 
                    break;
            }
        }

        private double PeepCallBlock(int recLocation)
        {
            switch (recLocation)
            {
                case 0:
                    return _callNumbersTop.Peek();
                case 1:
                    return _callNumbersBottom.Peek();
                case 2:
                    return _callNumbersLeft.Peek();
                default:
                    return _callNumbersRight.Peek();
            }
        }

        private double PopCallBlock(int recLocation)
        {
            _activatedBlockCount = 0;
            ClearAllFocus();
            switch (recLocation)
            {
                case 0:
                    return _callNumbersTop.Pop();
                case 1:
                    return _callNumbersBottom.Pop();
                case 2:
                    return _callNumbersLeft.Pop();
                default:
                    return _callNumbersRight.Pop();
            }
        }

        private void BtnReset_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _callNumbersBottom.Clear();
            _callNumbersTop.Clear();
            _callNumbersLeft.Clear();
            _callNumbersRight.Clear();

            InitializeStacks();
        }
        private void BtnSettings_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Change whatever you want
            txtMovesCount.Visibility = Visibility.Collapsed;
            btnReset.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Collapsed;

            btnSaveSettings.Visibility = Visibility.Visible;
            btnCloseSettings.Visibility = Visibility.Visible;
            //btnSettings.Source = new BitmapImage(new Uri(@"/Theme/Assets/Save.png", UriKind.Relative));;
            
            Canvas.SetLeft(txtTopRectStorageCapacity,322);
            Canvas.SetLeft(txtBottomRectStorageCapacity,322);
            Canvas.SetLeft(txtLeftRectStorageCapacity,174);
            Canvas.SetLeft(txtRightRectStorageCapacity,477);
            
            txtTopRectStorageCapacity.Content = "Size:";
            txtBottomRectStorageCapacity.Content = "Size:";
            txtLeftRectStorageCapacity.Content = "Size:";
            txtRightRectStorageCapacity.Content = "Size:";

            DockPanelTop.Visibility = Visibility.Visible;
            DockPanelBottom.Visibility = Visibility.Visible;
            DockPanelLeft.Visibility = Visibility.Visible;
            DockPanelRight.Visibility = Visibility.Visible;

            selectTopRect.Visibility = Visibility.Collapsed;
            selectBottomRect.Visibility = Visibility.Collapsed;
            selectLeftRect.Visibility = Visibility.Collapsed;
            selectRightRect.Visibility = Visibility.Collapsed;

            rectBlock1.Visibility = Visibility.Collapsed;
            rectBlock2.Visibility = Visibility.Collapsed;
            rectBlock3.Visibility = Visibility.Collapsed;
            rectBlock4.Visibility = Visibility.Collapsed;
            rectBlock5.Visibility = Visibility.Collapsed;
            rectBlock6.Visibility = Visibility.Collapsed;
            rectBlock7.Visibility = Visibility.Collapsed;
            rectBlock8.Visibility = Visibility.Collapsed;
            rectBlock9.Visibility = Visibility.Collapsed;
            rectBlock10.Visibility = Visibility.Collapsed;

            _onSettingsPage = true;
            UpdateDifficultyUiElements();
        }
        private void UpdateDifficultyUiElements()
        {
            if (_currentDifficulty > 5)
            {
                _currentDifficulty = 0;
              
            }
            _activeAscDesc = _preSetDifficulty.ChangeDifficulty(_currentDifficulty);
            switch (_currentDifficulty)
            {
                case 1:
                    UpdateStackSizeText(5, 5, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Normal.png", UriKind.Relative));
                    break;
                case 2:
                    UpdateStackSizeText(6, 6, 6, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Difficult.png", UriKind.Relative));
                    break;
                case 3:
                    UpdateStackSizeText(6, 6, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Hard.png", UriKind.Relative));
                    break;
                case 4:
                    UpdateStackSizeText(6, 5, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Extreme.png", UriKind.Relative));
                    break;
                case 5:
                    UpdateStackSizeText(5, 5, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Insane.png", UriKind.Relative));
                    break;
                default:
                    UpdateStackSizeText(6, 6, 6, 6);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Easy.png", UriKind.Relative));
                    break;;
            }

            UpdateArrows();
            
        }
        private void ImgDifficulty_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_onSettingsPage)
            {
                _currentDifficulty++;
                UpdateDifficultyUiElements();
            }
        }
        private void UpdateArrows()
        {
            //Top Rectangle pointing down
            imgTopRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
            imgBottomRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
            
            if (_activeAscDesc.Values.ElementAt(1))
            {
                imgTopRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));
            }
            else
            {
                imgTopRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }
            
            //Bottom Rectangle pointing down
            if (_activeAscDesc.Values.ElementAt(3))
            {
                imgBottomRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));
            }
            else
            {
                imgBottomRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }
            
            //Left Rectangle
            if (_activeAscDesc.Values.ElementAt(4))
            {
                imgLeftRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
            }
            else
            {
                imgLeftRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
            }
            
            if (_activeAscDesc.Values.ElementAt(5))
            {
                imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));
            }
            else
            {
                imgLeftRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }
            
            //Left Rectangle
            if (_activeAscDesc.Values.ElementAt(6))
            {
                imgRightRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpGreen.png", UriKind.Relative));
            }
            else
            {
                imgRightRectUp.Source = new BitmapImage(new Uri(@"/Theme/Assets/UpBlack.png", UriKind.Relative));
            }
            
            if (_activeAscDesc.Values.ElementAt(7))
            {
                imgRightRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownRed.png", UriKind.Relative));
            }
            else
            {
                imgRightRecDown.Source = new BitmapImage(new Uri(@"/Theme/Assets/DownBlack.png", UriKind.Relative));
            }

        }
        private void UpdateStackSizeText(int top,int bot,int left,int right)
        {
            StackSizeTop.Value = top;
            StackSizeBottom.Value = bot;
            StackSizeLeft.Value = left;
            StackSizeRight.Value = right;

            _stackSizes[0] = top;
            _stackSizes[1] = bot;
            _stackSizes[2] = left;
            _stackSizes[3] = right;
        }
        private void BtnSaveSettings_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void BtnCloseSettings_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtMovesCount.Visibility = Visibility.Visible;
            btnReset.Visibility = Visibility.Visible;
            btnSettings.Visibility = Visibility.Visible;

            btnSaveSettings.Visibility = Visibility.Collapsed;
            btnCloseSettings.Visibility = Visibility.Collapsed;
            
            Canvas.SetLeft(txtTopRectStorageCapacity,332);
            Canvas.SetLeft(txtBottomRectStorageCapacity,332);
            Canvas.SetLeft(txtLeftRectStorageCapacity,184);
            Canvas.SetLeft(txtRightRectStorageCapacity,487);
            
            txtTopRectStorageCapacity.Content = (_callNumbersTop.Count / _stackSizes[0])*100 + "%";
            txtBottomRectStorageCapacity.Content = (_callNumbersBottom.Count / _stackSizes[1])*100 + "%";
            txtLeftRectStorageCapacity.Content = (_callNumbersLeft.Count / _stackSizes[2])*100 + "%";
            txtRightRectStorageCapacity.Content = (_callNumbersRight.Count / _stackSizes[3])*100 + "%";
            
            DockPanelTop.Visibility = Visibility.Collapsed;
            DockPanelBottom.Visibility = Visibility.Collapsed;
            DockPanelLeft.Visibility = Visibility.Collapsed;
            DockPanelRight.Visibility = Visibility.Collapsed;
            
            selectTopRect.Visibility = Visibility.Visible;
            selectBottomRect.Visibility = Visibility.Visible;
            selectLeftRect.Visibility = Visibility.Visible;
            selectRightRect.Visibility = Visibility.Visible;

            rectBlock1.Visibility = Visibility.Visible;
            rectBlock2.Visibility = Visibility.Visible;
            rectBlock3.Visibility = Visibility.Visible;
            rectBlock4.Visibility = Visibility.Visible;
            rectBlock5.Visibility = Visibility.Visible;
            rectBlock6.Visibility = Visibility.Visible;
            rectBlock7.Visibility = Visibility.Visible;
            rectBlock8.Visibility = Visibility.Visible;
            rectBlock9.Visibility = Visibility.Visible;
            rectBlock10.Visibility = Visibility.Visible;

            _onSettingsPage = false;
        }
        private void TStackSizeTop_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TStackSizeTop.Text == "")
            {
                TStackSizeTop.Text = "5";
            }
        }
        private void TStackSizeTop_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (int.TryParse(TStackSizeTop.Text, out int value)) return;
            StackSizeTop.Value = 5;
            TStackSizeTop.Text = "5";
            e.Handled = true;
        }
        private void StackSizeTop_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _stackSizes[0] = StackSizeTop.Value;
        }
        private void TStackSizeBottom_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TStackSizeBottom.Text == "")
            {
                TStackSizeBottom.Text = "5";
            }
        }
        private void TStackSizeBottom_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (int.TryParse(TStackSizeBottom.Text, out int value)) return;
            StackSizeBottom.Value = 5;
            TStackSizeBottom.Text = "5";
            e.Handled = true;
        }
        private void StackSizeBottom_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _stackSizes[1] = StackSizeBottom.Value;
        }
        private void TStackSizeLeft_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TStackSizeLeft.Text == "")
            {
                TStackSizeLeft.Text = "5";
            }
        }
        private void TStackSizeLeft_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (int.TryParse(TStackSizeLeft.Text, out int value)) return;
            StackSizeLeft.Value = 5;
            TStackSizeLeft.Text = "5";
            e.Handled = true;
        }
        private void StackSizeLeft_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _stackSizes[2] = StackSizeLeft.Value;
        }
        private void TStackSizeRight_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TStackSizeRight.Text == "")
            {
                TStackSizeRight.Text = "5";
            }
        }
        private void TStackSizeRight_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (int.TryParse(TStackSizeRight.Text, out int value)) return;
            StackSizeRight.Value = 5;
            TStackSizeRight.Text = "5";
            e.Handled = true;
        }
        private void StackSizeRight_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _stackSizes[3] = StackSizeRight.Value;
        }
        private void imgLeftRecDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void imgBottomRectUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_onSettingsPage)
            {
                
            }
        }
        private void imgRightRecDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void imgRightRectUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void imgLeftRectUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void imgTopRecDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void imgTopRectUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void imgBottomRecDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}