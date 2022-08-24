using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace JoshMkhariPROG7312Game.Views
{
    /// <summary>
    /// Interaction logic for ReplaceBooksView.xaml
    /// </summary>
    public partial class ReplaceBooksView : UserControl
    {
        int activatedBlockCount;

        int[][] rectangles_arr = new int[4][]; //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
                                               // Creating 2 Stacks of Integers https://www.tutorialsteacher.com/csharp/csharp-stack
        
        Stack<int> callNumbersTop = new Stack<int>();//To store initial numbers in top rectangle
        Stack<int> callNumbersBottom = new Stack<int>();//To store initial numbers in bottom rectangle

        public ReplaceBooksView()
        {
            InitializeComponent();
            activatedBlockCount = 0;
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
                    bool Found;//To continue generating numbers until a number that does not exist in the list is generated
                    do
                    {
                        if (callNumbers.Contains(randomNum))//Check if random number exists
                        {
                            randomNum = rnd.Next(1000);//If it does, generate a new one
                            Found = true;
                        }
                        else
                        {
                            callNumbers.Add(randomNum);//Add the value that was not found
                            Found = false;//Stop the do while
                        }
                    } while (Found);

                }
            } while (callNumbers.Count < 10);//Repeat until 10 random numbers have been stored

            bool isEmptyRect;
            bool topFull = false;
            bool botFull = false;
            do
            {
                int rectChoice = rnd.Next(2);//stores random integers < 2
                if (rectChoice == 0)//Top rectangle was chosen
                {
                    isEmptyRect = !callNumbersTop.Any();//check if the list is empty
                    if (isEmptyRect)
                    {
                        callNumbersTop.Push(rnd.Next(1000));//add a value
                    }
                    else
                    {
                        if (callNumbersTop.Count < 5)//Check that there are less than 5 elements
                        {
                            int location = rnd.Next(callNumbers.Count - 1);//Choose a random number within the list
                            callNumbersTop.Push(callNumbers.ElementAt(location));//Push random number to the rectangle list
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
                    isEmptyRect = !callNumbersBottom.Any();//check if the list is empty
                    if (isEmptyRect)
                    {
                        callNumbersBottom.Push(rnd.Next(1000));//add a value
                    }
                    else
                    {
                        if (callNumbersBottom.Count < 5)//Check that there are less than 5 elements
                        {
                            int location = rnd.Next(callNumbers.Count - 1);//Choose a random number within the list
                            callNumbersBottom.Push(callNumbers.ElementAt(location));//Push random number to the rectangle list
                            callNumbers.RemoveAt(location);//Remove the number that was just pushed
                        }
                        else
                        {
                            botFull = true;
                        }
                    }
                }

            } while (!topFull || !botFull);//Stop when either the top rect or bottom rect is full

            if (topFull)//if the top one was the stack to be full https://stackoverflow.com/questions/19141259/how-to-enqueue-a-list-of-items-in-c
            {
                callNumbers.ForEach(o => callNumbersBottom.Push(o));//store the remaining numbers within this stack
            }

            else
            {
                callNumbers.ForEach(o => callNumbersTop.Push(o));////store the remaining numbers within this stack
            }
            callNumbers.Clear();

            //https://www.geeksforgeeks.org/stack-toarray-method-in-java-with-example/
            rectangles_arr[0] = callNumbersTop.ToArray();
            rectangles_arr[1] = callNumbersBottom.ToArray();

        }



        private void ReplacingBooks_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void selectTopRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (activatedBlockCount == 0)//This is start block
            {

                //check num of blocks within rect

                activatedBlockCount++;//Program now knows start, waiting for destination
            }
            else //this is destination block
            {

            }
            //Call method to activate block
        }

        private void selectBottomRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void selectRightRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void selectLeftRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
