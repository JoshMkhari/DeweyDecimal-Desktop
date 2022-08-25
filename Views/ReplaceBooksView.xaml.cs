using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private int _callRecToMove;

        private Double[] _rectCanvasLocationTop = new Double[10];//To store Top location of blocks 
        private Double[] _rectCanvasLocationLeft = new Double[10];//To store Left location of blocks
        
        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        private IDictionary<int, int> rectValueNamePair = new Dictionary<int, int>();//Stores Random value and Rectangle name
        public ReplaceBooksView()
        {
            
            InitializeComponent();
            _callRecToMove = 0;
            _activatedBlockCount = 0;
            
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

        private void ReplacingBooks_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void selectTopRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isEmptyRect = !_callNumbersTop.Any();//check if the list is empty
            if (_activatedBlockCount == 0)//This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect)//No more blocks to move
                {
                    SolidColorBrush blackBrush = new SolidColorBrush(Colors.Red);
                    TopRectStrokeChange(blackBrush);
                }
                else
                {
                    _callRecToMove = 0;//Represents Top
                    SolidColorBrush blackBrush = new SolidColorBrush(Colors.Gold);
                    this.RegisterName("MySolidColorBorderBrush", blackBrush);
                    selectTopRect.StrokeThickness = 3;
                    selectTopRect.Stroke = blackBrush;
                }
                
                _activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {
                //check num of blocks within rect
                
                if (isEmptyRect)//can add block
                {
                    _callNumbersTop.Push(PopCallBlock(_callRecToMove));
                }
                else
                {
                    if (_callNumbersTop.Count < 5)//If there is still space for the block
                    {
                        if (_callNumbersTop.Count == 1)//If there is only one block inside
                        { 
                            _callNumbersTop.Push(PopCallBlock(_callRecToMove)); //add second block
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
                            if (PopCallBlock(_callRecToMove) > _callNumbersTop.Peek())//If new addition is greater than top num
                            {
                                _callNumbersTop.Push(PopCallBlock(_callRecToMove)); //add block
                            }
                            else
                            {
                                SolidColorBrush blackBrush = new SolidColorBrush(Colors.Red);
                                TopRectStrokeChange(blackBrush);
                            }
                            //What is my order
                            if (_rectOrders[0])//If ascending
                            {
                                if (PopCallBlock(_callRecToMove) > _callNumbersTop.Peek())//If new addition is greater than top num
                                {
                                    _callNumbersTop.Push(PopCallBlock(_callRecToMove)); //add block
                                }
                                else
                                {
                                    SolidColorBrush blackBrush = new SolidColorBrush(Colors.Red);
                                    TopRectStrokeChange(blackBrush);
                                }
                            }
                            //Does it match my order?
                        }
                    }
                    else
                    {
                        SolidColorBrush blackBrush = new SolidColorBrush(Colors.Red);
                        TopRectStrokeChange(blackBrush);
                    }
                }
            }
        }

        private void TopRectStrokeChange(SolidColorBrush blackBrush)
        {
            this.RegisterName("MySolidColorBorderBrush", blackBrush);
            selectTopRect.StrokeThickness = 3;
            selectTopRect.Stroke = blackBrush;
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
                    _callRecToMove = 1;//Represents Bottom
                }
                
                _activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {
                //check num of blocks within rect
                
                if (isEmptyRect)//can add block
                {
                    _callNumbersBottom.Push(PopCallBlock(_callRecToMove));
                }
                else
                {
                    if (_callNumbersBottom.Count < 5)//If there is still space for the block
                    {
                        if (PopCallBlock(_callRecToMove) > _callNumbersTop.Peek())//If new addition is greater than top num
                        {
                            _callNumbersTop.Push(PopCallBlock(_callRecToMove)); //add block
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
                    _callRecToMove = 3;//Represents Right
                }
                
                _activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {
                //check num of blocks within rect
                
                if (isEmptyRect)//can add block
                {
                    SolidColorBrush blackBrush = new SolidColorBrush(Colors.Blue);
                    //this.RegisterName("MySolidColorBorderBrush", blackBrush);
                    selectRightRect.StrokeThickness = 3;
                    selectRightRect.Stroke = blackBrush;
                    
                    _callNumbersRight.Push(PopCallBlock(_callRecToMove));
                    //Call animation (Move blocks)
                    AnimateBlockMovement(3);
                    MessageBox.Show("top" + _callNumbersTop.Count + " vs right " + _callNumbersRight.Peek() +
                                    " with count " + _callNumbersRight.Count);
                }
                else
                {
                    if (_callNumbersRight.Count < 5)//If there is still space for the block
                    {
                        if (_callNumbersRight.Count == 1)//If there is only one block inside
                        { 
                            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Blue);
                            this.RegisterName("MySolidColorBorderBrush", blackBrush);
                            selectRightRect.StrokeThickness = 3;
                            selectRightRect.Stroke = blackBrush;
                            _callNumbersRight.Push(PopCallBlock(_callRecToMove)); //add second block
                            if (_callNumbersRight.ElementAt(0) > _callNumbersRight.ElementAt(1))
                            {
                                _rectOrders[1] = false;//Store Descending for Right Rectangle
                            }
                            else
                            {
                                _rectOrders[1] = true;//Store Ascending for Right Rectangle
                            }
                            //Call animation (Move blocks)
                            
                            MessageBox.Show("top" + _callNumbersTop.Count + " vs right " + _callNumbersRight.Peek() +
                                            " with count " + _callNumbersRight.Count);
                        }
                        else
                        {
                            if (PopCallBlock(_callRecToMove) > _callNumbersRight.Peek())//If new addition is greater than top num
                            {
                                _callNumbersRight.Push(PopCallBlock(_callRecToMove)); //add block
                            }
                            else
                            {
                                MessageBox.Show("This is an ascending list only X");
                            }
                            //What is my order
                            if (_rectOrders[0])//If ascending
                            {
                                if (PopCallBlock(_callRecToMove) > _callNumbersRight.Peek())//If new addition is greater than top num
                                {
                                    _callNumbersRight.Push(PopCallBlock(_callRecToMove)); //add block
                                }
                                else
                                {
                                    MessageBox.Show("This is currently an ascending list");
                                }
                            }
                            else
                            {
                                if (PopCallBlock(_callRecToMove) < _callNumbersRight.Peek())//If new addition is smaller than top num
                                {
                                    _callNumbersRight.Push(PopCallBlock(_callRecToMove)); //add block
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
            switch (rectStack.Count)
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
        //Store locations where blocks must rest based on amount of items within rectangle
        private int[] topRectCanvasYLocations = new[] { 198, 171, 144, 117, 0 }; //For top rectangle
        private int[] bottomRectCanvasYLocations = new[] { 422, 396, 369, 342, 315 };//For bottom rectangle

        private void MoveRectangle(int rectangleNumber)
        {
            switch (rectangleNumber)
            {
                case 1:
                    StartJourney(rectBlock1);
                    break;
                case 2:
                    StartJourney(rectBlock2);
                    break;
                case 3:
                    StartJourney(rectBlock3);
                    break;
                case 4:
                    StartJourney(rectBlock4);
                    break;
                case 5:
                    StartJourney(rectBlock5);
                    break;
                case 6:
                    StartJourney(rectBlock6);
                    break;
                case 7:
                    StartJourney(rectBlock7);
                    break;
                case 8:
                    StartJourney(rectBlock8);
                    break;
                case 9:
                    StartJourney(rectBlock9);
                    break;
                case 10:
                    StartJourney(rectBlock10);
                    break;
            }
        }

        //https://stackoverflow.com/questions/10298216/moving-any-control-in-wpf
        public void StartJourney(Border border)
        {
            do
            {
                Canvas.SetTop(border,Canvas.GetTop(border) -1);
                
            } while (Canvas.GetTop(border)>60);

            
        }
        private void AnimateBlockMovement(int destination)
        {
            int canvasLeft,canvasTop;
            switch (_callRecToMove)//Determine origin
            {
                case 0://From Top
                    //Calculate steps to top of rectangle
                
                //Check current amount of elements in rectangle
                    canvasTop = topRectCanvasYLocations[_callNumbersTop.Count-1];//Retrieves Y location of the rectangle on top
                    
                    //Now find out which rectangle it is

                    for (int i = 0; i < rectValueNamePair.Count; i++)
                    {
                        if (rectValueNamePair.Keys.ElementAt(i)==_callNumbersTop.Peek())
                        {
                            MoveRectangle(rectValueNamePair.Values.ElementAt(i));
                            break;
                        }
                    }


                    canvasLeft = 322; //Location on x axis where block must stop within top and bottom rectangles
                    break;
                
                //Check if going left, right, down
                //return _callNumbersTop.Pop();
                
                case 1://For bottom rectangle
                    canvasLeft = 322;//Location on x axis where block must stop within top and bottom rectangles
                    canvasTop = bottomRectCanvasYLocations[_callNumbersBottom.Count-1];
                    break;
                    //return _callNumbersBottom.Pop();
                
                case 2://For Left rectangle
                    canvasTop = ReturnCurrentBlockYLeftRight(_callNumbersLeft);
                    //return _callNumbersLeft.Pop();
                    canvasLeft = 175;//Location on x axis where block must stop within left rectangle 
                    break;
                
                default://For right rectangle
                    canvasTop = ReturnCurrentBlockYLeftRight(_callNumbersRight);
                    canvasLeft = 478;//Location on x axis where block must stop within right rectangle 
                    break;
                //return _callNumbersRight.Pop();
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
                    _callRecToMove = 2;//Represents Left
                }
                
                _activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {
                //check num of blocks within rect
                
                if (isEmptyRect)//can add block
                {
                    _callNumbersLeft.Push(PopCallBlock(_callRecToMove));
                }
                else
                {
                    if (_callNumbersLeft.Count < 5)//If there is still space for the block
                    {
                        if (_callNumbersLeft.Count == 1)//If there is only one block inside
                        { 
                            _callNumbersLeft.Push(PopCallBlock(_callRecToMove)); //add second block
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
                            if (PopCallBlock(_callRecToMove) > _callNumbersLeft.Peek())//If new addition is greater than top num
                            {
                                _callNumbersLeft.Push(PopCallBlock(_callRecToMove)); //add block
                            }
                            else
                            {
                                MessageBox.Show("This is an ascending list only X");
                            }
                            //What is my order
                            if (_rectOrders[0])//If ascending
                            {
                                if (PopCallBlock(_callRecToMove) > _callNumbersLeft.Peek())//If new addition is greater than top num
                                {
                                    _callNumbersLeft.Push(PopCallBlock(_callRecToMove)); //add block
                                }
                                else
                                {
                                    MessageBox.Show("This is currently an ascending list");
                                }
                            }
                            else
                            {
                                if (PopCallBlock(_callRecToMove) < _callNumbersLeft.Peek())//If new addition is smaller than top num
                                {
                                    _callNumbersLeft.Push(PopCallBlock(_callRecToMove)); //add block
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
