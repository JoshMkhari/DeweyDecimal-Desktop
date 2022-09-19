using System;
using System.Collections.Generic;
using System.Windows.Media;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;

namespace JoshMkhariPROG7312Game.ViewModels
{
    internal class ReplaceBooksViewModel
    {
        public bool OnSettingsPage { get; set; }
        
        private int _movesCount; //gameCounts 0
        private int _activatedBlockCount;//gameCounts 1
        
        private readonly int[] gameCounts ;//Stores both the moves made count and current amount of rectangles that are active
        
        
        public int CurrentDifficulty { get; set; } //0 for easy, 5 for insane

        private readonly int[] rectangleNumber ;//Stores both the destination rectangleNumber and originalRectangleNumber
        
        //You only need to store 1 and + 27
        private readonly int[] _bottomRectCanvasYLocations = { 422, 396, 369, 342, 315, 288 }; //For bottom rectangle 
        private readonly int[] _topRectCanvasYLocations = { 198, 171, 144, 117, 90, 63}; //For top rectangle
        
        private readonly char[] _rectangleSortOrder = new char [4]; //Store whether rectangle is ascending or descending
        private readonly double[] _stackSizes = new double[4];//Stores the capacity of each rectangle stack
        
        private SolidColorBrush _blackBrush;//Used to change rectangle selection colour

        private Stack<double> _callNumbers, _callNumbersTop, _callNumbersBottom, _callNumbersLeft, _callNumbersRight; //To store initial numbers in top rectangle
        private List<Stack<double>> CallNumberStacks{ get; set; }//Holds all relevant stackd
        
        private List<String> CallNumbersStrings { get; set; }
       
        private PreSetDifficulty _preSetDifficulty = new PreSetDifficulty();
        
        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        private readonly IDictionary<double, int>
            _rectValueNamePair = new Dictionary<double, int>(); //Stores Random value and Rectangle name
        private IDictionary<int, bool>
            _activeAscDesc; //Stores Set difficulty for current game

        ReplaceBooksViewModel()
        {
            CurrentDifficulty = 0;
            rectangleNumber = new[] { 4, 4 };//Origin and Destination
            //rectangleNumber 0 The rectangle that is receiving the number
            //rectangleNumber 1 The rectangle that is sending the number
            _activeAscDesc = _preSetDifficulty.ChangeDifficulty(CurrentDifficulty);
            _originalRectangleNumber = 4;
            _activatedBlockCount = 0;
            _rectangleSortOrder[0] = 'A';
            _rectangleSortOrder[1] = 'A';
            _movesCount = 0;
        }
    }
}