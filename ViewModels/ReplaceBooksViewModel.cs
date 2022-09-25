using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;

namespace JoshMkhariPROG7312Game.ViewModels
{
    public class ReplaceBooksViewModel
    {
        public ReplaceBooksViewModel()
        {
            CurrentDifficulty = 0;
            RectangleNumber = new[] { 4, 4 }; //Origin and Destination
            //rectangleNumber 0 The rectangle that is receiving the number
            //rectangleNumber 1 The rectangle that is sending the number
            BottomRectCanvasYLocations = new[] { 422, 396, 369, 342, 315, 288 }; //For bottom rectangle 
            TopRectCanvasYLocations = new[] { 198, 171, 144, 117, 90, 63 }; //For top rectangle
            CallNumbersStringValues = new List<int>();
            CallNumbers = new List<double>();
            CallNumbersStrings = new List<string>();
            RectValueNamePair = new Dictionary<double, int>();
            RectangleSortOrder = new[] { 'A', 'A', 'A', 'A' };
            StackSizes = new[] { 6.0, 6.0, 6.0, 6.0 };

            GameCounts = new[] { 0, 0 }; //movesCount, activatedCount
            //_movesCount keeps track of amount of total moves
            //_activatedBlockCount keeps track of current num of blocks that are active

            //To store initial numbers in relevant rectangle
            //0 = top
            //1 = bottom
            //2 = left
            //3 = right
            CallNumberStacks = new List<Stack<double>>();
            for (var i = 0; i < 4; i++) CallNumberStacks.Add(new Stack<double>());

            PreSetDiff = new PreSetDifficulty();
            ActiveAscDescStacks = PreSetDiff.ChangeDifficulty(CurrentDifficulty); //Determine sort order for each stack

            //RectangleSortOrder[0] = 'A';
            //RectangleSortOrder[1] = 'A';

            InitializeStacks();
        }

        public int[] GameCounts { get; } //Move count, BlockCount

        //Stores both the moves made count and current amount of rectangles that are active
        public int CurrentDifficulty { get; set; } //0 for easy, 5 for insane

        public int[]
            RectangleNumber { get; } //Stores both the originalRectangleNumber rectangleNumber and destination

        //You only need to store 1 and + 27
        public int[] BottomRectCanvasYLocations { get; } //For bottom rectangle 
        public int[] TopRectCanvasYLocations { get; } //For top rectangle

        public char[] RectangleSortOrder { get; } //Store whether rectangle is ascending or descending
        public double[] StackSizes { get; } //Stores the capacity of each rectangle stack

        public SolidColorBrush BlackBrush { get; set; } //Used to change rectangle selection colour
        public List<Stack<double>> CallNumberStacks { get; } //Holds all relevant stacks

        public List<double> CallNumbers { get; }
        public List<string> CallNumbersStrings { get; }
        public List<int> CallNumbersStringValues { get; }

        private PreSetDifficulty PreSetDiff { get; }

        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        public IDictionary<double, int>
            RectValueNamePair { get; } //Stores Random value and Rectangle name

        public IDictionary<int, bool>
            ActiveAscDescStacks { get; private set; } //Stores Set difficulty for current game

        private void InitializeStacks()
        {
            //To generate random numbers https://www.tutorialsteacher.com/articles/generate-random-numbers-in-csharp
            var rnd = new Random();

            for (var i = 0; i < 10; i++)
            {
                CallNumbers.Add((rnd.Next(1, 99999 * 10 + 1) + 1 * 10) / 1000.0); //add a value
                int num1 = (char)rnd.Next(65, 90);
                int num2 = (char)rnd.Next(65, 90);
                int num3 = (char)rnd.Next(65, 90);
                CallNumbersStringValues.Add(num1 + num2 + num3);
                CallNumbersStrings.Add(" " + (char)num1 + (char)num2 + (char)num3);
                //https://stackoverflow.com/questions/27531759/generating-decimal-random-numbers-in-java-in-a-specific-range
            }

            for (var i = 0; i < 10; i++)
                RectValueNamePair.Add(CallNumbers.ElementAt(i), i + 1); //storing the value with the rectangle name

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
            GameCounts[1] = 0; //Active Block Count
        }

        public void UpdateDifficulty()
        {
            ActiveAscDescStacks = PreSetDiff.ChangeDifficulty(CurrentDifficulty); //Determine sort order for each stack
        }
    }
}