using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JoshMkhariPROG7312Game.Views
{
    /// <summary>
    /// Interaction logic for ReplaceBooksView.xaml
    /// </summary>
    public partial class ReplaceBooksView : UserControl
    {
        private int _activatedBlockCount;

        private char[] _rectangleSortOrder = new char [4];//Store whether rectangle is ascending or descending
        //private int[][] _rectanglesArr = new int[4][]; //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        // Creating 2 Stacks of Integers https://www.tutorialsteacher.com/csharp/csharp-stack
        
        private Stack<int> _callNumbersTop = new Stack<int>();//To store initial numbers in top rectangle
        private Stack<int> _callNumbersBottom = new Stack<int>();//To store initial numbers in bottom rectangle
        private Stack<int> _callNumbersLeft = new Stack<int>();//To store initial numbers in left rectangle
        private Stack<int> _callNumbersRight = new Stack<int>();//To store initial numbers in right rectangle
        
        //Store locations where blocks must rest based on amount of items within rectangle
        private int[] _topRectCanvasYLocations = { 198, 171, 144, 117, 0 }; //For top rectangle
        private int[] _bottomRectCanvasYLocations = { 422, 396, 369, 342, 315 };//For bottom rectangle
        
        private int _originRectangleNumber;//??????????????????????????????????????
        private int _destinationRectangleNumber;//The rectangle a block is being transferred to

        private SolidColorBrush _blackBrush;

        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        private IDictionary<int, int> _rectValueNamePair = new Dictionary<int, int>();//Stores Random value and Rectangle name
        public ReplaceBooksView()
        {
            
            InitializeComponent();
            _originRectangleNumber = 4;
            _activatedBlockCount = 0;
            
            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Blue);
            this.RegisterName("MySolidColorBorderBrush", blackBrush);

            Stack<int> callNumbers = new Stack<int>();//To store initial 10 values
            //To generate random numbers https://www.tutorialsteacher.com/articles/generate-random-numbers-in-csharp
            Random rnd = new Random();
            do
            {
                int randomNum = rnd.Next(1000);
                bool isEmpty = !callNumbers.Any();//check if the list is empty
                if (isEmpty)
                {
                    callNumbers.Push(randomNum);//add a value
                }
                else
                {
                    bool found;//To continue generating numbers until a number that does not exist in the list is generated
                    do
                    {
                        if (callNumbers.Contains(randomNum))//Check if random number exists
                        {
                            randomNum = rnd.Next(1000);//If it does, generate a new one
                            found = true;
                        }
                        else
                        {
                            callNumbers.Push(randomNum);//Add the value that was not found
                            found = false;//Stop the do while
                        }
                    } while (found);

                }
            } while (callNumbers.Count < 10);//Repeat until 10 random numbers have been stored

            for (int i = 0; i < 5; i++)
            {
                _callNumbersTop.Push(callNumbers.ElementAt(i));
            }            
            for (int i = 5; i < 10; i++)
            {
                _callNumbersBottom.Push(callNumbers.ElementAt(i));
            }
            
            callNumbers.Clear();

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
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(4),1); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(3),2); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(2),3); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(1),4); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersTop.ElementAt(0),5); //storing the value with the rectangle name

            
            txtRectBlock6.Text = _callNumbersBottom.ElementAt(4).ToString();
            txtRectBlock7.Text = _callNumbersBottom.ElementAt(3).ToString();
            txtRectBlock8.Text = _callNumbersBottom.ElementAt(2).ToString();
            txtRectBlock9.Text = _callNumbersBottom.ElementAt(1).ToString();
            txtRectBlock10.Text = _callNumbersBottom.ElementAt(0).ToString();
            
            //https://www.tutorialsteacher.com/csharp/csharp-dictionary
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(0),6); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(1),7); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(2),8); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(3),9); //storing the value with the rectangle name
            _rectValueNamePair.Add(_callNumbersBottom.ElementAt(4),10); //storing the value with the rectangle name
            
            Debug.WriteLine("Top");
            foreach (var VARIABLE in _callNumbersTop)
            {
                Debug.WriteLine(VARIABLE + ' ');
            }            
            Debug.WriteLine(" ");
            Debug.WriteLine("Bottom");
            foreach (var VARIABLE in _callNumbersBottom)
            {
                Debug.WriteLine(VARIABLE);
            }       
            Debug.WriteLine(" ");
            Debug.WriteLine("Left");
            foreach (var VARIABLE in _callNumbersLeft)
            {
                Debug.WriteLine(VARIABLE + ' ');
            }    
            Debug.WriteLine(" ");
            Debug.WriteLine("Right");
            foreach (var VARIABLE in _callNumbersRight)
            {
                Debug.WriteLine(VARIABLE + ' ');
            }
            Debug.WriteLine(" ");
        }
        
        //To colour block strokes
        private void ActivateBlockColour(Rectangle rect, int mode)
        {
            switch (mode)
            {
                case 0:
                    _blackBrush = new SolidColorBrush(Colors.Gold);//Sender
                    _activatedBlockCount++;//Program now knows start, waiting for destination
                    break;
                case 1:
                    _blackBrush = new SolidColorBrush(Colors.Blue);//Reciever
                    break;
                case 2:
                    _blackBrush = new SolidColorBrush(Colors.Red);//Reciever
                    break;
                default:
                    _blackBrush = new SolidColorBrush(Colors.Transparent);//Error
                    break;
            }
            rect.StrokeThickness = 3;
            rect.Stroke = _blackBrush;
        }
        private void ReplacingBooks_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        //To make a block show red when an error occurs
        private void ActivateRedError(Rectangle rect,string errorText)
        {
            ActivateBlockColour(rect,2);//Display Red
            MessageBox.Show("Action not allowed " + errorText);
            ClearAllFocus();
            _activatedBlockCount = 0;
        }
        //To clear all colours surrounding blocks
        private void ClearAllFocus()
        {
            ActivateBlockColour(selectTopRect,3);//Make Transparent
            ActivateBlockColour(selectBottomRect,3);//Make Transparent
            ActivateBlockColour(selectLeftRect,3);//Make Transparent
            ActivateBlockColour(selectRightRect,3);//Make Transparent
            _activatedBlockCount = 0;
            _destinationRectangleNumber = 0;
        }

        //To push a block number from one stack to another
        private void UpdateStack(int rectangleNumber)
        {
            AnimateBlockMovement();
            switch (rectangleNumber)
            {
                case 0:
                    _callNumbersTop.Push(PopCallBlock(_originRectangleNumber));//Push to top stack
                    Debug.WriteLine("Pushed a block to top");
                    break;                
                case 1:
                    _callNumbersBottom.Push(PopCallBlock(_originRectangleNumber));//Push to bottom stack
                    Debug.WriteLine("Pushed a block to bottom");
                    break;                
                case 2:
                    _callNumbersLeft.Push(PopCallBlock(_originRectangleNumber));//Push to left stack
                    Debug.WriteLine("Pushed a block to left");
                    break;                
                case 3:
                    _callNumbersRight.Push(PopCallBlock(_originRectangleNumber));//Push to right stack
                    Debug.WriteLine("Pushed a block to right");
                    break;
            }

            if (_callNumbersLeft.Count > 1)
            {
                if (_callNumbersLeft.ElementAt(0) < _callNumbersLeft.ElementAt(1))
                {
                    _rectangleSortOrder[2] = 'D';//Store Descending for Left Rectangle
                    Debug.WriteLine("Left block is descending");
                }
                else
                {
                    _rectangleSortOrder[2] = 'A';//Store Ascending for Left Rectangle
                    Debug.WriteLine("Left block is ascending");
                }
            }            
            if (_callNumbersRight.Count > 1)
            {
                if (_callNumbersRight.ElementAt(0) < _callNumbersRight.ElementAt(1))
                {
                    _rectangleSortOrder[3] = 'D';//Store Descending for Right Rectangle
                    Debug.WriteLine("Right block is descending");
                }
                else
                {
                    _rectangleSortOrder[3] = 'A';//Store Ascending for Right Rectangle
                    Debug.WriteLine("Right block is ascending");
                }
            }
        }
        private void SelectedRectangle(Rectangle currentRectangle, Stack<int> currentRectangleStack, int rectangleNumber)
        {
            var isEmptyRect = !currentRectangleStack.Any();//check if the list is empty
            if (_activatedBlockCount == 0)//This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect)//No more blocks to move
                {
                    ActivateRedError(currentRectangle,"This rectangle is empty, cannot send anything from it");
                }
                else
                {
                    _originRectangleNumber = rectangleNumber;//Represents 
                    ActivateBlockColour(currentRectangle,0);//Display Gold as this is start block
                }
            }
            else //this is destination block
            {
                //check num of blocks within rect
                if (isEmptyRect)//can add block
                {
                    if (_originRectangleNumber != rectangleNumber)//Make sure the source and the destination are not the same
                    {
                        ActivateBlockColour(currentRectangle,1);//Make Blue
                        _destinationRectangleNumber = rectangleNumber;//Set destination 
                        Debug.WriteLine("if (isEmptyRect) _destinationRectangleNumber " + rectangleNumber);
                        UpdateStack(rectangleNumber);
                    }
                    else
                    {
                        ActivateRedError(currentRectangle,"You cannot send a call number to the same start");
                    }
                }
                else
                {
                    if (currentRectangleStack.Count < 5)//If there is still space for the block
                    {
                        if (currentRectangleStack.Count == 1)//If there is only one block inside
                        { 
                            ActivateBlockColour(currentRectangle,1);//Make Blue
                            _destinationRectangleNumber = rectangleNumber;//Set destination 
                            Debug.WriteLine("if (currentRectangleStack.Count == 1) _destinationRectangleNumber " + rectangleNumber);
                            UpdateStack(rectangleNumber);
                        }
                        else
                        {
                            Debug.WriteLine(" ");
                            Debug.WriteLine("originRectangle Number " + _originRectangleNumber);
                            Debug.WriteLine("Current Rectangle Number " + rectangleNumber);
                            //What is my order
                            if (_rectangleSortOrder[rectangleNumber] == 'A')//If ascending
                            {
                                if (PeepCallBlock(_originRectangleNumber) > PeepCallBlock(rectangleNumber))//If new addition is greater than top num
                                {
                                    ActivateBlockColour(currentRectangle,1);//Make Blue
                                    _destinationRectangleNumber = rectangleNumber;//Set destination 
                                    Debug.WriteLine("if Asscending _destinationRectangleNumber " + rectangleNumber);
                                    UpdateStack(rectangleNumber);
                                }
                                else
                                {
                                    Debug.WriteLine(" ");
                                    Debug.WriteLine("You tried to add " +  PeepCallBlock(_originRectangleNumber) + " which is smaller than " + PeepCallBlock(rectangleNumber));
                                    ActivateRedError(currentRectangle,"This is a ascending list");
                                }
                            }else if (_rectangleSortOrder[rectangleNumber] == 'D') //If Descending
                            {
                                if (PeepCallBlock(_originRectangleNumber) < PeepCallBlock(rectangleNumber)) //If new addition is greater than top num
                                {
                                    ActivateBlockColour(currentRectangle,1);//Make Blue
                                    _destinationRectangleNumber = rectangleNumber;//Set destination 
                                    Debug.WriteLine("if Descending _destinationRectangleNumber " + rectangleNumber);
                                    UpdateStack(rectangleNumber);
                                }
                                else
                                {
                                    Debug.WriteLine(" ");
                                    Debug.WriteLine("You tried to add " +  PeepCallBlock(_originRectangleNumber) + " which is greater than " + PeepCallBlock(rectangleNumber));
                                    ActivateRedError(currentRectangle,"This is a descending list");
                                }
                            }

                            //Does it match my order?
                        }
                    }
                    else
                    {
                        ActivateRedError(currentRectangle,"Full");
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
        private int ReturnCurrentBlockYLeftRight(Stack<int> rectStack)
        {
            switch (rectStack.Count+1)
            {
                case 1://Rectangle at bottom of rect list
                    return 320;
                case 2://Second rectangle
                    return 293;
                case 3:
                    return 266;
                case 4:
                    return 239;
                default://Last rectangle
                    return 213;
            }
        }


        private void MoveRectangle(int rectangleNumber, int originRectTop)
        {
            switch (rectangleNumber)
            {
                case 1:
                    StartYJourney(rectBlock1,originRectTop);
                    break;
                case 2:
                    StartYJourney(rectBlock2,originRectTop);
                    break;
                case 3:
                    StartYJourney(rectBlock3,originRectTop);
                    break;
                case 4:
                    StartYJourney(rectBlock4,originRectTop);
                    break;
                case 5:
                    StartYJourney(rectBlock5,originRectTop);
                    break;
                case 6:
                    StartYJourney(rectBlock6,originRectTop);
                    break;
                case 7:
                    StartYJourney(rectBlock7,originRectTop);
                    break;
                case 8:
                    StartYJourney(rectBlock8,originRectTop);
                    break;
                case 9:
                    StartYJourney(rectBlock9,originRectTop);
                    break;
                case 10:
                    StartYJourney(rectBlock10,originRectTop);
                    break;
            }
        }

        private void StartXJourneyRight(Border border, int stop)
        {
           // Debug.WriteLine("This is left now " +Canvas.GetLeft(border));
            do
            {
                Canvas.SetLeft(border,Canvas.GetLeft(border) + 1);
                
            } while (Canvas.GetLeft(border)<stop);
        }
        private void StartXJourneyLeft(Border border, int stop)
        {
           // Debug.WriteLine("This is left now " + Canvas.GetLeft(border));
            do
            {
                Canvas.SetLeft(border,Canvas.GetLeft(border) - 1);
                
            } while (Canvas.GetLeft(border)>stop);
        }
        private void StartYJourneyUp(Border border)
        {
            int destinationY = 0;
            Debug.WriteLine("_destinationRectangleNumber " + _destinationRectangleNumber);
            switch (_destinationRectangleNumber)
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersTop,_topRectCanvasYLocations);
                    break;
                case 1:// For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersBottom,_bottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    break;
                case 3:// For right block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    break;
  
            }
            //Debug.WriteLine("this is destination Y " + destinationY);
           // Debug.WriteLine("this is current Y " + Canvas.GetTop(border));
            do
            {
                Canvas.SetTop(border,Canvas.GetTop(border) +1);
                
            } while (Canvas.GetTop(border)<destinationY);//285<320
            
            //285<218
            //285>320
            
            //285>320

            if (Canvas.GetTop(border) > destinationY) //320 <320  285< 218
            {
                do
                {
                    Canvas.SetTop(border,Canvas.GetTop(border) -1);
                
                } while (Canvas.GetTop(border)>destinationY); //285>320  285>218
            }
            //Check destination amount of elements
        }
        private void StartYJourneyDown(Border border)
        {
            int destinationY = 0;
            Debug.WriteLine("_destinationRectangleNumber " + _destinationRectangleNumber);
            switch (_destinationRectangleNumber)
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersTop,_topRectCanvasYLocations);
                    break;
                case 1:// For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersBottom,_bottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    break;
                case 3:// For right block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    break;
  
            }
            //Debug.WriteLine("this is destination Y" + destinationY);
            do
            {
                Canvas.SetTop(border,Canvas.GetTop(border) +1);
                
            } while (Canvas.GetTop(border)<destinationY);
            //Check destination amount of elements
            
        }

        //https://stackoverflow.com/questions/10298216/moving-any-control-in-wpf
        private void StartYJourney(Border border, int top)
        {
            //Move up based on current location to top of current rectangle
            do
            {
                Canvas.SetTop(border,Canvas.GetTop(border) -1);
                
            } while (Canvas.GetTop(border)>top);
            
            //Determine if need to be going left or right

            Debug.WriteLine("_destinationRectangleNumber " + _destinationRectangleNumber);
            switch (_destinationRectangleNumber)//0 Top, 1 Bottom, 2 Left, 3 Right
            {
                case 0 : //Going to top
                    switch (_originRectangleNumber)
                    {
                        case 1://Starting from bottom
                            StartYJourneyUp(border); //Just go straight up
                            break;
                        case 2://Starting from left
                            StartXJourneyRight(border,322);
                            //Then go right until reach 322
                            break;
                        case 3://Starting from right
                            StartXJourneyLeft(border,322);
                        //Then go left until reach 322
                            break;
                    }
                    break;
                case 1 : //Going to Bottom 285
                    switch (_originRectangleNumber)
                    {
                        case 0://Starting from Top
                            //Just go straight down
                            StartYJourneyDown(border);
                            break;
                        case 2://Starting from left
                            //Go right until reach 322
                            StartXJourneyRight(border,322);
                        //Then go down until reach saved location
                            break;
                        case 3://Starting from right
                            //Go left until reach 322
                            StartXJourneyLeft(border,322);
                        //Then go down reach saved location
                            break;
                    }
                    break;
                case 2 : //Going to Left 180
                    switch (_originRectangleNumber)
                    {
                        case 0://Starting from top
                            //Go left until 175
                            StartXJourneyLeft(border,175);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 1://Starting from bottom
                            StartXJourneyLeft(border,175);
                            //Then go down reach saved location
                            StartYJourneyUp(border);
                            break;
                        case 3://Starting from right
                            StartXJourneyLeft(border,175);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }
                    break;
                case 3 : //Going to Right one
                    switch (_originRectangleNumber)
                    { //478
                        case 0://Starting from top
                            //Go right until 478
                            StartXJourneyRight(border,478);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 1://Starting from bottom
                            //Go up until top of right and left
                            //Go right until 478
                            StartXJourneyRight(border,478);
                            //Then go down reach saved location
                            StartYJourneyUp(border);
                            break;
                        case 2://Starting from left
                            //Go right until 478
                            StartXJourneyRight(border,478);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }
                    break;
            }
            
        }

        private int ReturnCurrentBlockYTopBottom(Stack<int> rectStack, int[] locations)
        {
            int location = rectStack.Count - 1;
            if (location < 0)
                location++;

            return locations[location];
        }

        private void AnimateBlockMovement()
        {
            switch (_originRectangleNumber)//Determine origin
            {
                case 0://From Top
                    
                    for (int i = 0; i < _rectValueNamePair.Count; i++)
                    {
                        if (_rectValueNamePair.Keys.ElementAt(i)==_callNumbersTop.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i),60);
                            break;
                        }
                    }
                    //canvasLeft = 322; //Location on x axis where block must stop within top and bottom rectangles
                    break;
                
                case 1://For bottom rectangle 285
                    //canvasLeft = 322;//Location on x axis where block must stop within top and bottom rectangles
                    for (int i = 0; i < _rectValueNamePair.Count; i++)
                    {
                        if (_rectValueNamePair.Keys.ElementAt(i)==_callNumbersBottom.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i),285);
                            break;
                        }
                    }
                    break;

                case 2://For Left rectangle
                    //canvasTop = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    //return _callNumbersLeft.Pop();
                    for (int i = 0; i < _rectValueNamePair.Count; i++)
                    {
                        if (_rectValueNamePair.Keys.ElementAt(i)==_callNumbersLeft.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i),180);
                            break;
                        }
                    }
                    //canvasLeft = 175;//Location on x axis where block must stop within left rectangle 
                    break;
                default:
                    for (int i = 0; i < _rectValueNamePair.Count; i++)
                    {
                        if (_rectValueNamePair.Keys.ElementAt(i)==_callNumbersRight.Peek())
                        {
                            MoveRectangle(_rectValueNamePair.Values.ElementAt(i),180);
                            break;
                        }
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
