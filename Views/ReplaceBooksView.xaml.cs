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

        private bool[] _rectOrders = new bool [4];//Store whether rectangle is ascending or descending
        private int[][] _rectanglesArr = new int[4][]; //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        // Creating 2 Stacks of Integers https://www.tutorialsteacher.com/csharp/csharp-stack
        
        private Stack<int> _callNumbersTop = new Stack<int>();//To store initial numbers in top rectangle
        private Stack<int> _callNumbersBottom = new Stack<int>();//To store initial numbers in bottom rectangle
        private Stack<int> _callNumbersLeft = new Stack<int>();//To store initial numbers in left rectangle
        private Stack<int> _callNumbersRight = new Stack<int>();//To store initial numbers in right rectangle
        
        private int _originRectangleNumber;//??????????????????????????????????????
        private int _destinationRectangleNumber;
        private int _yLocation;

        private SolidColorBrush _blackBrush;
        private Double[] _rectCanvasLocationTop = new Double[10];//To store Top location of blocks 
        private Double[] _rectCanvasLocationLeft = new Double[10];//To store Left location of blocks
        
        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        private IDictionary<int, int> rectValueNamePair = new Dictionary<int, int>();//Stores Random value and Rectangle name
        public ReplaceBooksView()
        {
            
            InitializeComponent();
            _originRectangleNumber = 4;
            _activatedBlockCount = 0;
            
            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Blue);
            this.RegisterName("MySolidColorBorderBrush", blackBrush);
            
            //Storing current locations of all rectangles
            _rectCanvasLocationTop[0] = Canvas.GetTop(rectBlock1);
            _rectCanvasLocationTop[1] = Canvas.GetTop(rectBlock2);
            _rectCanvasLocationTop[2] = Canvas.GetTop(rectBlock3);
            _rectCanvasLocationTop[3] = Canvas.GetTop(rectBlock4);
            _rectCanvasLocationTop[4] = Canvas.GetTop(rectBlock5);
            _rectCanvasLocationTop[5] = Canvas.GetTop(rectBlock6);
            _rectCanvasLocationTop[6] = Canvas.GetTop(rectBlock7);
            _rectCanvasLocationTop[7] = Canvas.GetTop(rectBlock8);
            _rectCanvasLocationTop[8] = Canvas.GetTop(rectBlock9);
            _rectCanvasLocationTop[9] = Canvas.GetTop(rectBlock10);
            
            _rectCanvasLocationLeft[0] = Canvas.GetLeft(rectBlock1);
            _rectCanvasLocationLeft[1] = Canvas.GetLeft(rectBlock2);
            _rectCanvasLocationLeft[2] = Canvas.GetLeft(rectBlock3);
            _rectCanvasLocationLeft[3] = Canvas.GetLeft(rectBlock4);
            _rectCanvasLocationLeft[4] = Canvas.GetLeft(rectBlock5);
            _rectCanvasLocationLeft[5] = Canvas.GetLeft(rectBlock6);
            _rectCanvasLocationLeft[6] = Canvas.GetLeft(rectBlock7);
            _rectCanvasLocationLeft[7] = Canvas.GetLeft(rectBlock8);
            _rectCanvasLocationLeft[8] = Canvas.GetLeft(rectBlock9);
            _rectCanvasLocationLeft[9] = Canvas.GetLeft(rectBlock10);
            
            List<int> callNumbers = new List<int>();//To store initial 10 values
            //To generate random numbers https://www.tutorialsteacher.com/articles/generate-random-numbers-in-csharp
            Random rnd = new Random();
            do
            {
                int randomNum = rnd.Next(1000);
                bool isEmpty = !callNumbers.Any();//check if the list is empty
                if (isEmpty)
                {
                    callNumbers.Add(randomNum);//add a value
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
                            callNumbers.Add(randomNum);//Add the value that was not found
                            found = false;//Stop the do while
                        }
                    } while (found);

                }
            } while (callNumbers.Count < 10);//Repeat until 10 random numbers have been stored

            
            bool topFull = false;
            bool botFull = false;
            do
            {
                bool isEmptyRect;
                int rectChoice = rnd.Next(2);//stores random integers < 2
                if (rectChoice == 0)//Top rectangle was chosen
                {
                    isEmptyRect = !_callNumbersTop.Any();//check if the list is empty
                    if (isEmptyRect)
                    {
                        _callNumbersTop.Push(rnd.Next(1000));//add a value
                    }
                    else
                    {
                        if (_callNumbersTop.Count < 5)//Check that there are less than 5 elements
                        {
                            int location = rnd.Next(callNumbers.Count - 1);//Choose a random number within the list
                            _callNumbersTop.Push(callNumbers.ElementAt(location));//Push random number to the rectangle list
                            callNumbers.RemoveAt(location);//Remove the number that was just pushed
                        }
                        else
                        {
                            topFull = true;
                        }
                    }
                }
                else
                {
                    isEmptyRect = !_callNumbersBottom.Any();//check if the list is empty
                    if (isEmptyRect)
                    {
                        _callNumbersBottom.Push(rnd.Next(1000));//add a value
                    }
                    else
                    {
                        if (_callNumbersBottom.Count < 5)//Check that there are less than 5 elements
                        {
                            int location = rnd.Next(callNumbers.Count - 1);//Choose a random number within the list
                            _callNumbersBottom.Push(callNumbers.ElementAt(location));//Push random number to the rectangle list
                            callNumbers.RemoveAt(location);//Remove the number that was just pushed
                        }
                        else
                        {
                            botFull = true;
                        }
                    }
                }

            } while (!topFull || !botFull);//Stop when either the top rect or bottom rect is full
            Console.WriteLine("We make it this far");
            if (topFull)//if the top one was the stack to be full https://stackoverflow.com/questions/19141259/how-to-enqueue-a-list-of-items-in-c
            {
                callNumbers.ForEach(o => _callNumbersBottom.Push(o));//store the remaining numbers within this stack
            }

            else
            {
                callNumbers.ForEach(o => _callNumbersTop.Push(o));////store the remaining numbers within this stack
            }
            callNumbers.Clear();

            //https://www.geeksforgeeks.org/stack-toarray-method-in-java-with-example/
            _rectanglesArr[0] = _callNumbersTop.ToArray();
            _rectanglesArr[1] = _callNumbersBottom.ToArray();
            _rectanglesArr[2] = new int[5];
            _rectanglesArr[3] = new int[5];

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
            rectValueNamePair.Add(_callNumbersTop.ElementAt(4),1); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersTop.ElementAt(3),2); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersTop.ElementAt(2),3); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersTop.ElementAt(1),4); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersTop.ElementAt(0),5); //storing the value with the rectangle name

            
            txtRectBlock6.Text = _callNumbersBottom.ElementAt(4).ToString();
            txtRectBlock7.Text = _callNumbersBottom.ElementAt(3).ToString();
            txtRectBlock8.Text = _callNumbersBottom.ElementAt(2).ToString();
            txtRectBlock9.Text = _callNumbersBottom.ElementAt(1).ToString();
            txtRectBlock10.Text = _callNumbersBottom.ElementAt(0).ToString();
            
            //https://www.tutorialsteacher.com/csharp/csharp-dictionary
            rectValueNamePair.Add(_callNumbersBottom.ElementAt(4),6); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersBottom.ElementAt(3),7); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersBottom.ElementAt(2),8); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersBottom.ElementAt(1),9); //storing the value with the rectangle name
            rectValueNamePair.Add(_callNumbersBottom.ElementAt(0),10); //storing the value with the rectangle name
            
            
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

        private void ActivateRedError(Rectangle rect)
        {
            ActivateBlockColour(rect,2);//Display Red
            Thread.Sleep(1000);// Keep Red for 1 second
            ActivateBlockColour(rect,3);//Make Transparent
        }
        private void selectTopRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isEmptyRect = !_callNumbersTop.Any();//check if the list is empty
            if (_activatedBlockCount == 0)//This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect)//No more blocks to move
                {
                    ActivateRedError(selectTopRect);
                }
                else
                {
                    _originRectangleNumber = 0;//Represents Top
                    ActivateBlockColour(selectTopRect,0);//Display Gold as this is start block
                }
            }
            else //this is destination block
            {
                //check num of blocks within rect
                if (isEmptyRect)//can add block
                {
                    if (_originRectangleNumber != 0)//Make sure the source and the destination are not the same
                    {
                        ActivateBlockColour(selectRightRect,1);//Make Blue
                        _destinationRectangleNumber = 0;//Set destination 
                        
                        AnimateBlockMovement();//Move block?????????????????????????????????????????????????????????????????????????????????????????????????????????????????
                        _callNumbersRight.Push(PopCallBlock(_originRectangleNumber));//Push from stack
                    
                        ActivateBlockColour(selectRightRect,3);//Make Transparent
                        _activatedBlockCount = 0;//reset activated block count
                    }
                    else
                    {
                        ActivateRedError(selectTopRect);
                    }
                }
                else
                {
                    if (_callNumbersTop.Count < 5)//If there is still space for the block
                    {
                        if (_callNumbersTop.Count == 1)//If there is only one block inside
                        { 
                            _callNumbersTop.Push(PopCallBlock(_originRectangleNumber)); //add second block
                            ActivateBlockColour(selectTopRect,3);//Make Transparent
                            if (_callNumbersTop.ElementAt(0) > _callNumbersTop.ElementAt(1))
                            {
                                _rectOrders[0] = false;//Store Descending for Top Rectangle
                            }
                            else
                            {
                                _rectOrders[0] = true;//Store Ascending for Top Rectangle
                            }
                        }
                        else
                        {
                            if (PopCallBlock(_originRectangleNumber) > _callNumbersTop.Peek())//If new addition is greater than top num
                            {
                                _callNumbersTop.Push(PopCallBlock(_originRectangleNumber)); //add block
                                ActivateBlockColour(selectTopRect,3);//Make Transparent
                            }
                            else
                            {
                                ActivateBlockColour(selectTopRect,2);//Display Red
                                Thread.Sleep(1000);// Keep Red for 1 second
                                ActivateBlockColour(selectTopRect,3);//Make Transparent
                            }
                            //What is my order
                            if (_rectOrders[0])//If ascending
                            {
                                if (PopCallBlock(_originRectangleNumber) > _callNumbersTop.Peek())//If new addition is greater than top num
                                {
                                    _callNumbersTop.Push(PopCallBlock(_originRectangleNumber)); //add block
                                    ActivateBlockColour(selectTopRect,3);//Make Transparent
                                }
                                else
                                {
                                    ActivateBlockColour(selectTopRect,2);//Display Red
                                    Thread.Sleep(1000);// Keep Red for 1 second
                                    ActivateBlockColour(selectTopRect,3);//Make Transparent
                                }
                            }
                            //Does it match my order?
                        }
                    }
                    else
                    {
                        ActivateBlockColour(selectTopRect,2);//Display Red
                        Thread.Sleep(1000);// Keep Red for 1 second
                        ActivateBlockColour(selectTopRect,3);//Make Transparent
                    }
                }
            }
        }

        private void selectBottomRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isEmptyRect = !_callNumbersBottom.Any();//check if the list is empty
            if (_activatedBlockCount == 0)//This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect)//No more blocks to move
                {
                    MessageBox.Show("Code X to appear and say empty");
                }
                else
                {
                    _originRectangleNumber = 1;//Represents Bottom
                }
                
                _activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {
                //check num of blocks within rect
                
                if (isEmptyRect)//can add block
                {
                    _callNumbersBottom.Push(PopCallBlock(_originRectangleNumber));
                }
                else
                {
                    if (_callNumbersBottom.Count < 5)//If there is still space for the block
                    {
                        if (PopCallBlock(_originRectangleNumber) > _callNumbersTop.Peek())//If new addition is greater than top num
                        {
                            _callNumbersTop.Push(PopCallBlock(_originRectangleNumber)); //add block
                        }
                        else
                        {
                            MessageBox.Show("This is an ascending list only X");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Space Error");
                    }
                }
            }
        }

        private void selectRightRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
            bool isEmptyRect = !_callNumbersRight.Any();//check if the list is empty
            if (_activatedBlockCount == 0)//This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect)//No more blocks to move
                {
                    MessageBox.Show("Code X to appear and say empty");
                }
                else
                {
                    _originRectangleNumber = 3;//Represents Right
                    ActivateBlockColour(selectRightRect,1);//Make Gold
                }
                
                _activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {
                //check num of blocks within rect
                
                if (isEmptyRect)//can add block
                {
                    ActivateBlockColour(selectRightRect,1);//Make Blue
                    
                    //Call animation (Move blocks)
                    _destinationRectangleNumber = 3;//Set destination 
                    AnimateBlockMovement();//Move block
                    _callNumbersRight.Push(PopCallBlock(_originRectangleNumber));//Push from stack
                    
                    ActivateBlockColour(selectRightRect,3);//Make Transparent
                    _activatedBlockCount = 0;//reset activated block count

                }
                else
                {
                    if (_callNumbersRight.Count < 5)//If there is still space for the block
                    {
                        if (_callNumbersRight.Count == 1)//If there is only one block inside
                        { 
                            ActivateBlockColour(selectRightRect,1);//Make Transparent
                            
                            //Call animation (Move blocks)
                            _destinationRectangleNumber = 3;//Set destination 
                            AnimateBlockMovement();//Move block
                            _callNumbersRight.Push(PopCallBlock(_originRectangleNumber));//Push from stack
                            ActivateBlockColour(selectRightRect,3);//Make Transparent
                            _activatedBlockCount = 0;//reset activated block count
                            
                            if (_callNumbersRight.ElementAt(0) > _callNumbersRight.ElementAt(1))
                            {
                                _rectOrders[1] = false;//Store Descending for Right Rectangle
                            }
                            else
                            {
                                _rectOrders[1] = true;//Store Ascending for Right Rectangle
                            }

                        }
                        else
                        {
                            if (PopCallBlock(_originRectangleNumber) > _callNumbersRight.Peek())//If new addition is greater than top num
                            {
                                ActivateBlockColour(selectRightRect,1);//Make Blue
                                //Call animation (Move blocks)
                                _destinationRectangleNumber = 3;//Set destination 
                                AnimateBlockMovement();//Move block
                                _callNumbersRight.Push(PopCallBlock(_originRectangleNumber)); //add block
                                ActivateBlockColour(selectRightRect,3);//Make Transparent
                            }
                            else
                            {
                                MessageBox.Show("This is an ascending list only X");
                                
                            }
                            //What is my order
                            if (_rectOrders[0])//If ascending
                            {
                                if (PopCallBlock(_originRectangleNumber) > _callNumbersRight.Peek())//If new addition is greater than top num
                                {
                                    //Call animation (Move blocks)
                                    ActivateBlockColour(selectRightRect,1);//Make Blue
                                    _destinationRectangleNumber = 3;//Set destination 
                                    AnimateBlockMovement();//Move block
                                    _callNumbersRight.Push(PopCallBlock(_originRectangleNumber)); //add block
                                    ActivateBlockColour(selectRightRect,3);//Make Transparent
                                }
                                else
                                {
                                    MessageBox.Show("This is currently an ascending list");
                                }
                            }
                            else
                            {
                                if (PopCallBlock(_originRectangleNumber) < _callNumbersRight.Peek())//If new addition is smaller than top num
                                {
                                    ActivateBlockColour(selectRightRect,1);//Make Blue
                                    _callNumbersRight.Push(PopCallBlock(_originRectangleNumber)); //add block
                                    ActivateBlockColour(selectRightRect,3);//Make Transparent
                                }
                                else
                                {
                                    MessageBox.Show("This is currently a descending list");
                                }
                            }
                            //Does it match my order?
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Space Error");
                    }
                }
            }
        }
        private void selectLeftRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isEmptyRect = !_callNumbersLeft.Any();//check if the list is empty
            if (_activatedBlockCount == 0)//This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect)//No more blocks to move
                {
                    MessageBox.Show("Code X to appear and say empty");
                }
                else
                {
                    _originRectangleNumber = 2;//Represents Left
                }
                
                _activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {
                //check num of blocks within rect
                
                if (isEmptyRect)//can add block
                {
                    _callNumbersLeft.Push(PopCallBlock(_originRectangleNumber));
                }
                else
                {
                    if (_callNumbersLeft.Count < 5)//If there is still space for the block
                    {
                        if (_callNumbersLeft.Count == 1)//If there is only one block inside
                        { 
                            _callNumbersLeft.Push(PopCallBlock(_originRectangleNumber)); //add second block
                            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Transparent);
                            if (_callNumbersLeft.ElementAt(0) > _callNumbersLeft.ElementAt(1))
                            {
                                _rectOrders[0] = false;//Store Descending for Left Rectangle
                            }
                            else
                            {
                                _rectOrders[0] = true;//Store Ascending for Left Rectangle
                            }
                        }
                        else
                        {
                            if (PopCallBlock(_originRectangleNumber) > _callNumbersLeft.Peek())//If new addition is greater than top num
                            {
                                _callNumbersLeft.Push(PopCallBlock(_originRectangleNumber)); //add block
                            }
                            else
                            {
                                MessageBox.Show("This is an ascending list only X");
                            }
                            //What is my order
                            if (_rectOrders[0])//If ascending
                            {
                                if (PopCallBlock(_originRectangleNumber) > _callNumbersLeft.Peek())//If new addition is greater than top num
                                {
                                    _callNumbersLeft.Push(PopCallBlock(_originRectangleNumber)); //add block
                                }
                                else
                                {
                                    MessageBox.Show("This is currently an ascending list");
                                }
                            }
                            else
                            {
                                if (PopCallBlock(_originRectangleNumber) < _callNumbersLeft.Peek())//If new addition is smaller than top num
                                {
                                    _callNumbersLeft.Push(PopCallBlock(_originRectangleNumber)); //add block
                                }
                                else
                                {
                                    MessageBox.Show("This is currently a descending list");
                                }
                            }
                            //Does it match my order?
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Space Error");
                    }
                }
            }
        }        
        //For both left and right rectangles
        private int ReturnCurrentBlockYLeftRight(Stack<int> rectStack)
        {
            Debug.WriteLine("we trying to figure out why this size aint changing " + rectStack.Count);
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
            Debug.WriteLine("This is left now " +Canvas.GetLeft(border));
            do
            {
                Canvas.SetLeft(border,Canvas.GetLeft(border) + 1);
                
            } while (Canvas.GetLeft(border)<stop);
        }
        private void StartXJourneyLeft(Border border, int stop)
        {
            Debug.WriteLine("This is left now " + Canvas.GetLeft(border));
            do
            {
                Canvas.SetLeft(border,Canvas.GetLeft(border) - 1);
                
            } while (Canvas.GetLeft(border)>stop);
        }
        private void StartYJourneyUp(Border border)
        {
            int destinationY = 0;
            switch (_destinationRectangleNumber)
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersTop,topRectCanvasYLocations);
                    break;
                case 1:// For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersBottom,bottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    break;
                case 3:// For right block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    break;
  
            }
            Debug.WriteLine("this is destination Y" + destinationY);
            do
            {
                Canvas.SetTop(border,Canvas.GetTop(border) -1);
                
            } while (Canvas.GetTop(border)>destinationY);
            //Check destination amount of elements
        }
        private void StartYJourneyDown(Border border)
        {
            int destinationY = 0;
            switch (_destinationRectangleNumber)
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersTop,topRectCanvasYLocations);
                    break;
                case 1:// For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_callNumbersBottom,bottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    break;
                case 3:// For right block
                    destinationY = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    break;
  
            }
            Debug.WriteLine("this is destination Y" + destinationY);
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
        //Store locations where blocks must rest based on amount of items within rectangle
        private int[] topRectCanvasYLocations = new[] { 198, 171, 144, 117, 0 }; //For top rectangle
        private int[] bottomRectCanvasYLocations = new[] { 422, 396, 369, 342, 315 };//For bottom rectangle

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
                    
                    for (int i = 0; i < rectValueNamePair.Count; i++)
                    {
                        if (rectValueNamePair.Keys.ElementAt(i)==_callNumbersTop.Peek())
                        {
                            MoveRectangle(rectValueNamePair.Values.ElementAt(i),60);
                            break;
                        }
                    }
                    //canvasLeft = 322; //Location on x axis where block must stop within top and bottom rectangles
                    break;
                
                case 1://For bottom rectangle 285
                    //canvasLeft = 322;//Location on x axis where block must stop within top and bottom rectangles
                    for (int i = 0; i < rectValueNamePair.Count; i++)
                    {
                        if (rectValueNamePair.Keys.ElementAt(i)==_callNumbersBottom.Peek())
                        {
                            MoveRectangle(rectValueNamePair.Values.ElementAt(i),285);
                            break;
                        }
                    }
                    break;

                case 2://For Left rectangle
                    //canvasTop = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    //return _callNumbersLeft.Pop();
                    for (int i = 0; i < rectValueNamePair.Count; i++)
                    {
                        if (rectValueNamePair.Keys.ElementAt(i)==_callNumbersLeft.Peek())
                        {
                            MoveRectangle(rectValueNamePair.Values.ElementAt(i),180);
                            break;
                        }
                    }
                    //canvasLeft = 175;//Location on x axis where block must stop within left rectangle 
                    break;
                default:
                    for (int i = 0; i < rectValueNamePair.Count; i++)
                    {
                        if (rectValueNamePair.Keys.ElementAt(i)==_callNumbersRight.Peek())
                        {
                            MoveRectangle(rectValueNamePair.Values.ElementAt(i),180);
                            break;
                        }
                    }
                    //For right rectangle
                    //canvasTop = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    //canvasLeft = 478;//Location on x axis where block must stop within right rectangle 
                    break;
            }
        }

        
        private int PopCallBlock(int recLocation)
                 {
                     _activatedBlockCount = 0;
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
