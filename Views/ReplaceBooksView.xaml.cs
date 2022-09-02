using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JoshMkhariPROG7312Game.Views
{
    /// <summary>
    ///     Interaction logic for ReplaceBooksView.xaml
    /// </summary>
    public partial class ReplaceBooksView : UserControl
    {
        private int _activatedBlockCount;

        private SolidColorBrush _blackBrush;
        private readonly int[] _bottomRectCanvasYLocations = { 422, 396, 369, 342, 315 }; //For bottom rectangle

            //private Stack<int> _callNumbersBottom = new Stack<int>(); //To store initial numbers in bottom rectangle

        //private Stack<int> _callNumbersLeft = new Stack<int>(); //To store initial numbers in left rectangle

        //private Stack<int> _callNumbersRight = new Stack<int>(); //To store initial numbers in right rectangle
        //private int[][] _rectanglesArr = new int[4][]; //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        // Creating 2 Stacks of Integers https://www.tutorialsteacher.com/csharp/csharp-stack

        private Stack<int> _callNumbersTop, _callNumbersBottom, _callNumbersLeft, _callNumbersRight; //To store initial numbers in top rectangle
        
        private int _destinationRectangleNumber; //The rectangle a block is being transferred to

        private int _originalRectangleNumber; //??????????????????????????????????????

        private readonly char[] _rectangleSortOrder = new char [4]; //Store whether rectangle is ascending or descending

        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        private readonly IDictionary<int, int>
            _rectValueNamePair = new Dictionary<int, int>(); //Stores Random value and Rectangle name

        //Store locations where blocks must rest based on amount of items within rectangle
        private readonly int[] _topRectCanvasYLocations = { 198, 171, 144, 117, 90 }; //For top rectangle

        private int _movesCount;
        public ReplaceBooksView()
        {
            InitializeComponent();
            _originalRectangleNumber = 4;
            _activatedBlockCount = 0;
            _rectangleSortOrder[0] = 'A';
            _rectangleSortOrder[1] = 'A';
            _movesCount = 0;
            
            var blackBrush = new SolidColorBrush(Colors.Blue);
            RegisterName("MySolidColorBorderBrush", blackBrush);

            var callNumbers = new Stack<int>(); //To store initial 10 values
            //To generate random numbers https://www.tutorialsteacher.com/articles/generate-random-numbers-in-csharp
            var rnd = new Random();

            callNumbers.Push(rnd.Next(1000)); //add a value
            do
            {
                bool found; //To continue generating numbers until a number that does not exist in the list is generated
                do
                {
                    var randomNum = rnd.Next(1000);
                    if (!callNumbers.Contains(randomNum)) //Check if random number exists
                    {
                        callNumbers.Push(randomNum); //Add the value that was not found
                        found = false; //Stop the do while
                    }
                    else
                    {
                        found = true; //continue do while
                    }
                } while (found);
            } while (callNumbers.Count < 10); //Repeat until 10 random numbers have been stored
            
            _callNumbersTop = new Stack<int>();
            _callNumbersBottom = new Stack<int>();
            _callNumbersLeft = new Stack<int>();
            _callNumbersRight = new Stack<int>();
            
            for (var i = 0; i < 5; i++) _callNumbersTop.Push(callNumbers.ElementAt(i));

            for (var i = 5; i < 10; i++) _callNumbersBottom.Push(callNumbers.ElementAt(i));
            

            //https://www.geeksforgeeks.org/stack-toarray-method-in-java-with-example/
            //_rectanglesArr[0] = _callNumbersTop.ToArray();
            // _rectanglesArr[1] = _callNumbersBottom.ToArray();
            // _rectanglesArr[2] = new int[5];
            //_rectanglesArr[3] = new int[5];

            AssignValuesToBlocks();
        }

        private void AssignValuesToBlocks()
        {
            txtRectBlock1.Text = _callNumbersTop.ElementAt(4).ToString();
            txtRectBlock2.Text = _callNumbersTop.ElementAt(3).ToString();
            txtRectBlock3.Text = _callNumbersTop.ElementAt(2).ToString();
            txtRectBlock4.Text = _callNumbersTop.ElementAt(1).ToString();
            txtRectBlock5.Text = _callNumbersTop.ElementAt(0).ToString();
            
            //https://www.tutorialsteacher.com/csharp/csharp-dictionary
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(4), 1); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(3), 2); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(2), 3); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(1), 4); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(0), 5); //storing the value with the rectangle name
            
            txtRectBlock6.Text = _callNumbersBottom.ElementAt(0).ToString();
            txtRectBlock7.Text = _callNumbersBottom.ElementAt(1).ToString();
            txtRectBlock8.Text = _callNumbersBottom.ElementAt(2).ToString();
            txtRectBlock8.Text = _callNumbersBottom.ElementAt(2).ToString();
            txtRectBlock9.Text = _callNumbersBottom.ElementAt(3).ToString();
            txtRectBlock10.Text = _callNumbersBottom.ElementAt(4).ToString();

            //https://www.tutorialsteacher.com/csharp/csharp-dictionary
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(0), 6); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(1), 7); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(2), 8); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(3), 9); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(4), 10); //storing the value with the rectangle name

            DisplayValuesInRectangles();
        }

        private void DisplayValuesInRectangles()
        {
            Debug.WriteLine("Top");
            foreach (var VARIABLE in _callNumbersTop) Debug.WriteLine(VARIABLE.ToString());
            Debug.WriteLine(" ");
            Debug.WriteLine("Bottom");
            foreach (var VARIABLE in _callNumbersBottom) Debug.WriteLine(VARIABLE.ToString());
            Debug.WriteLine(" ");
            Debug.WriteLine("Left");
            foreach (var VARIABLE in _callNumbersLeft) Debug.WriteLine(VARIABLE.ToString());
            Debug.WriteLine(" ");
            Debug.WriteLine("Right");
            foreach (var VARIABLE in _callNumbersRight) Debug.WriteLine(VARIABLE.ToString());
            Debug.WriteLine(" ");
        }

        //To colour block strokes
        private void ActivateBlockColour(Rectangle rect, int mode)
        {
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

            if (_callNumbersLeft.Count > 1)
            {
                if (_callNumbersLeft.ElementAt(0) < _callNumbersLeft.ElementAt(1))
                {
                    _rectangleSortOrder[2] = 'D'; //Store Descending for Left Rectangle
                    Debug.WriteLine("Left block is descending");
                }
                else
                {
                    _rectangleSortOrder[2] = 'A'; //Store Ascending for Left Rectangle
                    Debug.WriteLine("Left block is ascending");
                }
            }

            if (_callNumbersRight.Count > 1)
            {
                if (_callNumbersRight.ElementAt(0) < _callNumbersRight.ElementAt(1))
                {
                    _rectangleSortOrder[3] = 'D'; //Store Descending for Right Rectangle
                    Debug.WriteLine("Right block is descending");
                }
                else
                {
                    _rectangleSortOrder[3] = 'A'; //Store Ascending for Right Rectangle
                    Debug.WriteLine("Right block is ascending");
                }
            }
        }

        private void SelectedRectangle(Rectangle currentRectangle, Stack<int> currentRectangleStack,
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
                        Debug.WriteLine("if (isEmptyRect) _destinationRectangleNumber " + currentRectangleNumber);
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
                        if (currentRectangleStack.Count < 5) //If there is still space for the block
                        {
                            if (currentRectangleStack.Count == 1) //If there is only one block inside
                            {
                                ActivateBlockColour(currentRectangle, 1); //Make Blue
                                _destinationRectangleNumber = currentRectangleNumber; //Set destination 
                                Debug.WriteLine("if (currentRectangleStack.Count == 1) _destinationRectangleNumber " +
                                                currentRectangleNumber);
                                UpdateStack(currentRectangleNumber);
                            }
                            else
                            {
                                Debug.WriteLine(" ");
                                Debug.WriteLine("originRectangle Number " + _originalRectangleNumber);
                                Debug.WriteLine("Current Rectangle Number " + currentRectangleNumber);
                                //What is my order
                                if (_rectangleSortOrder[currentRectangleNumber] == 'A') //If ascending
                                {
                                    if (PeepCallBlock(_originalRectangleNumber) >
                                        PeepCallBlock(currentRectangleNumber)) //If new addition is greater than top num
                                    {
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _destinationRectangleNumber = currentRectangleNumber; //Set destination 
                                        Debug.WriteLine("if Asscending _destinationRectangleNumber " +
                                                        currentRectangleNumber);
                                        UpdateStack(currentRectangleNumber);
                                    }
                                    else
                                    {
                                        Debug.WriteLine(" ");
                                        Debug.WriteLine("You tried to add " + PeepCallBlock(_originalRectangleNumber) +
                                                        " which is smaller than " + PeepCallBlock(currentRectangleNumber));
                                        ActivateRedError(currentRectangle, "This is a ascending list");
                                    }
                                }
                                else if (_rectangleSortOrder[currentRectangleNumber] == 'D') //If Descending
                                {
                                    if (PeepCallBlock(_originalRectangleNumber) <
                                        PeepCallBlock(currentRectangleNumber)) //If new addition is greater than top num
                                    {
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _destinationRectangleNumber = currentRectangleNumber; //Set destination 
                                        Debug.WriteLine("if Descending _destinationRectangleNumber " +
                                                        currentRectangleNumber);
                                        UpdateStack(currentRectangleNumber);
                                    }
                                    else
                                    {
                                        Debug.WriteLine(" ");
                                        Debug.WriteLine("You tried to add " + PeepCallBlock(_originalRectangleNumber) +
                                                        " which is greater than " + PeepCallBlock(currentRectangleNumber));
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

            DisplayValuesInRectangles();
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
        private int ReturnCurrentBlockYLeftRight(Stack<int> rectStack)
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
                default: //Last rectangle
                    return 213;
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
            // Debug.WriteLine("This is left now " +Canvas.GetLeft(border));
            do
            {
                Canvas.SetLeft(border, Canvas.GetLeft(border) + 1);
            } while (Canvas.GetLeft(border) < stop);
        }

        private void StartXJourneyLeft(Border border, int stop)
        {
            // Debug.WriteLine("This is left now " + Canvas.GetLeft(border));
            do
            {
                Canvas.SetLeft(border, Canvas.GetLeft(border) - 1);
            } while (Canvas.GetLeft(border) > stop);
        }

        private void StartYJourneyUp(Border border)
        {
            var destinationY = 0;
            Debug.WriteLine("_destinationRectangleNumber nigga please" + _destinationRectangleNumber);
            switch (_destinationRectangleNumber)
            {
                case 0: //For Top Block
                    Debug.WriteLine("We going up");
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersTop, _topRectCanvasYLocations);
                    break;
                case 1: // For Bottom block
                    Debug.WriteLine("We down up");
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersBottom, _bottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                    Debug.WriteLine("We going left");
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    break;
                case 3: // For right block
                    Debug.WriteLine("We going right");
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    break;
            }

            //Debug.WriteLine("this is destination Y " + destinationY);
            // Debug.WriteLine("this is current Y " + Canvas.GetTop(border));
            Debug.WriteLine("0 ");
            Debug.WriteLine("Current location " + Canvas.GetTop(border) + " with a destination of " + destinationY);
            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) + 1);
            } while (Canvas.GetTop(border) < destinationY); //285<320
            Debug.WriteLine("1 ");
            Debug.WriteLine("Current location " + Canvas.GetTop(border) + " with a destination of " + destinationY);
            if (Canvas.GetTop(border) > destinationY) //320 <320  285< 218
                do
                {
                    Canvas.SetTop(border, Canvas.GetTop(border) - 1);
                } while (Canvas.GetTop(border) > destinationY); //285>320  285>218
            Debug.WriteLine("2 ");
            Debug.WriteLine("Current location " + Canvas.GetTop(border) + " with a destination of " + destinationY);
            //Check destination amount of elements
        }

        private void StartYJourneyDown(Border border)
        {
            var destinationY = 0;
            Debug.WriteLine("_destinationRectangleNumber nigga please " + _destinationRectangleNumber);
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

            //Debug.WriteLine("this is destination Y" + destinationY);
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

            Debug.WriteLine("_destinationRectangleNumber before YJourney " + _destinationRectangleNumber);
            switch (_destinationRectangleNumber) //0 Top, 1 Bottom, 2 Left, 3 Right
            {
                case 0: //Going to top
                    switch (_originalRectangleNumber)
                    {
                        case 1: //Starting from bottom
                            Debug.WriteLine("We moving top from bottom");
                            StartYJourneyUp(border); //Just go straight up
                            break;
                        case 2: //Starting from left
                            
                            StartXJourneyRight(border, 322);
                            //Then go right until reach 322
                            StartYJourneyUp(border); //Just go straight up
                            break;
                        case 3: //Starting from right
                            StartXJourneyLeft(border, 322);
                            //Then go left until reach 322
                            StartYJourneyUp(border); //Just go straight up
                            break;
                    }
                    break;
                case 1: //Going to Bottom 285
                    switch (_originalRectangleNumber)
                    {
                        case 0: //Starting from Top
                            //Just go straight down
                            Debug.WriteLine("We moving down from top");
                            StartYJourneyDown(border);
                            break;
                        case 2: //Starting from left
                            //Go right until reach 322
                            StartXJourneyRight(border, 322);
                            //Then go down until reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 3: //Starting from right
                            //Go left until reach 322
                            StartXJourneyLeft(border, 322);
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

        private int ReturnCurrentBlockYTopBottom(Stack<int> rectStack, int[] locations)
        {
            Debug.WriteLine("");
            
            int location; //To store where to place the block
            if (rectStack.Any())//If there are elements within the stack
            {
                location = rectStack.Count;
            }
            else
                location = 0;
            Debug.WriteLine("Num items in stack " + rectStack.Count);
            Debug.WriteLine("Location to be sent to is  " + location);
            Debug.WriteLine("To move to this location " + locations[location]);
            Debug.WriteLine("");
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

                    //canvasLeft = 322; //Location on x axis where block must stop within top and bottom rectangles
                    break;

                case 1: //From bottom rectangle
                    //canvasLeft = 322;//Location on x axis where block must stop within top and bottom rectangles
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

        private int PeepCallBlock(int recLocation)
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

        private int PopCallBlock(int recLocation)
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
    }
}