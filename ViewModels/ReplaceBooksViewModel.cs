using System;
using System.Collections.Generic;
using System.Windows.Media;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;

namespace JoshMkhariPROG7312Game.ViewModels
{
    internal class ReplaceBooksViewModel
    {
        public bool OnSettingsPage { get; set; }//True if current page is settings page
        private int[] GameCounts { get; set; }//Stores both the moves made count and current amount of rectangles that are active
        public int CurrentDifficulty { get; set; } //0 for easy, 5 for insane
        public int[] RectangleNumber { get; set; }//Stores both the destination rectangleNumber and originalRectangleNumber
        
        //You only need to store 1 and + 27
        private readonly int[] _bottomRectCanvasYLocations = { 422, 396, 369, 342, 315, 288 }; //For bottom rectangle 
        private readonly int[] _topRectCanvasYLocations = { 198, 171, 144, 117, 90, 63}; //For top rectangle
        
        private char[] RectangleSortOrder { get; set; } //Store whether rectangle is ascending or descending
        private double[] StackSizes { get; set; }//Stores the capacity of each rectangle stack
        
        public SolidColorBrush BlackBrush{ get; set; }//Used to change rectangle selection colour
        public List<Stack<double>> CallNumberStacks{ get; set; }//Holds all relevant stackd
        
        public List<double> CallNumbers { get; set;}
        public List<String> CallNumbersStrings { get; set;}
       
        public PreSetDifficulty PreSetDiff { get; set;}
        
        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        private readonly IDictionary<double, int>
            _rectValueNamePair = new Dictionary<double, int>(); //Stores Random value and Rectangle name
        private IDictionary<int, bool>
            ActiveAscDescStacks{ get; set;} //Stores Set difficulty for current game

        public ReplaceBooksViewModel()
        {
            OnSettingsPage = false;
            CurrentDifficulty = 0;
            RectangleNumber = new[] { 4, 4 };//Origin and Destination
            //rectangleNumber 0 The rectangle that is receiving the number
            //rectangleNumber 1 The rectangle that is sending the number

            RectangleSortOrder = new char[4];
            StackSizes = new double[4];
            
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
            
            RectangleSortOrder[0] = 'A';
            RectangleSortOrder[1] = 'A';
        }
    }
}