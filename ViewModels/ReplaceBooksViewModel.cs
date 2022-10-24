using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;

namespace JoshMkhariPROG7312Game.ViewModels
{
    public class ReplaceBooksViewModel
    {
        public int[] GameCounts { get; set; }//Move count, BlockCount
        //Stores both the moves made count and current amount of rectangles that are active
        public int CurrentDifficulty { get; set; } //0 for easy, 5 for insane
        public int[] RectangleNumber { get; set; }//Stores both the originalRectangleNumber rectangleNumber and destination
        
        //You only need to store 1 and + 27
        public int[] BottomRectCanvasYLocations { get; set; }//For bottom rectangle 
        public int[] TopRectCanvasYLocations { get; set; } //For top rectangle
        
        public char[] RectangleSortOrder { get; set; } //Store whether rectangle is ascending or descending
        public double[] StackSizes { get; set; }//Stores the capacity of each rectangle stack
        
        public SolidColorBrush BlackBrush{ get; set; }//Used to change rectangle selection colour
        public List<Stack<double>> CallNumberStacks{ get; set; }//Holds all relevant stacks
        
        public List<double> CallNumbers { get; set;}
        public List<String> CallNumbersStrings { get; set;}
        public List<int> CallNumbersStringValues { get; set; }

        private PreSetDifficulty PreSetDiff { get; set;}
        
        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        public IDictionary<double, int>
            RectValueNamePair{ get; set;} //Stores Random value and Rectangle name
        public IDictionary<int, bool>
            ActiveAscDescStacks{ get; private set;} //Stores Set difficulty for current game

        public ReplaceBooksViewModel()
        {
            CurrentDifficulty = 0;
            RectangleNumber = new[] { 4, 4 };//Origin and Destination
            //rectangleNumber 0 The rectangle that is receiving the number
            //rectangleNumber 1 The rectangle that is sending the number
            BottomRectCanvasYLocations =new[] { 422, 396, 369, 342, 315, 288 }; //For bottom rectangle 
            TopRectCanvasYLocations = new[]{ 198, 171, 144, 117, 90, 63}; //For top rectangle
            CallNumbersStringValues = new List<int>();
            CallNumbers = new List<double>();
            CallNumbersStrings = new List<string>();
            RectValueNamePair = new Dictionary<double, int>();
            RectangleSortOrder = new[] { 'A', 'A', 'A', 'A' };
            StackSizes = new[] { 6.0, 6.0, 6.0, 6.0 };
            
            GameCounts = new []{0,0};//movesCount, activatedCount
            //_movesCount keeps track of amount of total moves
            //_activatedBlockCount keeps track of current num of blocks that are active
            
            //To store initial numbers in relevant rectangle
            //0 = top
            //1 = bottom
            //2 = left
            //3 = right
            CallNumberStacks = new List<Stack<double>>();
            for (int i = 0; i < 4; i++)
            {
                CallNumberStacks.Add(new Stack<double>());
            }

            PreSetDiff = new PreSetDifficulty();
            ActiveAscDescStacks = PreSetDiff.ChangeDifficulty(CurrentDifficulty);//Determine sort order for each stack
            
            //RectangleSortOrder[0] = 'A';
            //RectangleSortOrder[1] = 'A';

            InitializeStacks();
        }
        
        private void InitializeStacks()
        {
            //To generate random numbers https://www.tutorialsteacher.com/articles/generate-random-numbers-in-csharp
            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                CallNumbers.Add((rnd.Next(1,((99999)*10+1))+1*10)/1000.0); //add a value
                int num1 = (char)rnd.Next(65, 90);
                int num2 = (char)rnd.Next(65, 90);
                int num3 = (char)rnd.Next(65, 90);
                CallNumbersStringValues.Add(num1+num2+num3);
                CallNumbersStrings.Add(" " +(char) num1 + (char) num2 + (char) num3);
                //https://stackoverflow.com/questions/27531759/generating-decimal-random-numbers-in-java-in-a-specific-range
            }
            
            for (int i = 0; i < 10; i++)
            {
                RectValueNamePair.Add(CallNumbers.ElementAt(i), i+1); //storing the value with the rectangle name
            }

            InitializeTopAndBottomStacks();
        }

        public void InitializeTopAndBottomStacks()
        {
            CallNumberStacks.ElementAt(0).Clear();
            CallNumberStacks.ElementAt(1).Clear();
            CallNumberStacks.ElementAt(2).Clear();
            CallNumberStacks.ElementAt(3).Clear();
            for (var i = 0; i < 5; i++) CallNumberStacks.ElementAt(1).Push(CallNumbers.ElementAt(i));
            
            for (var i = 5; i < 10; i++) CallNumberStacks.ElementAt(0).Push(CallNumbers.ElementAt(i));
        }

        public void PushCallNumber(int destinationStack, int originStack)
        {
            CallNumberStacks.ElementAt(destinationStack).Push(CallNumberStacks.ElementAt(originStack).Pop());
            GameCounts[1] = 0;//Active Block Count
        }

        public void UpdateDifficulty()
        {
            ActiveAscDescStacks = PreSetDiff.ChangeDifficulty(CurrentDifficulty);//Determine sort order for each stack
        }
    }
}