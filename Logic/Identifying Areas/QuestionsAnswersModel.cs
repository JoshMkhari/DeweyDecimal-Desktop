using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace JoshMkhariPROG7312Game.Logic.Identifying_Areas
{
    public class QuestionsAnswersModel
    {
        //https://www.tutorialsteacher.com/csharp/csharp-dictionary
        public IDictionary<string, int> _ChosenSet{ get; set;} //Stores Random Answer and Question Pairs;

        private List<string> _descriptonsList;
        private List<int> _numbersList;

        public QuestionsAnswersModel(List<double> numbers)
        {
            _descriptonsList = new List<string>();
            _numbersList = new List<int>();
            _ChosenSet = new Dictionary<string, int>();
            
            PopulateDescriptions();
            PopulateNumbers();
            PopulateChosenSet(numbers);

        }

        private void PopulateChosenSet(List<double> numbers)
        {
            for (int i = 0; i < 4; i++)
            {
                double changed = Math.Floor(numbers.ElementAt(i));
                int workWith = (int)Math.Round(changed - 50);
                int rounded = ((workWith + 99) / 100 ) * 100;
                bool found = false;
                for (int j = 0; j < _numbersList.Count; j++)
                {
                    if (rounded == _numbersList.ElementAt(j))
                    {
                        if (!found)
                        {
                            _ChosenSet.Add(_descriptonsList.ElementAt(j),_numbersList.ElementAt(j));
                            found = true;
                        }
                        _numbersList.RemoveAt(j);
                        _descriptonsList.RemoveAt(j);
                    }
                }
                
            }

            for (int i = 0; i < 3; i++)
            {
                var rnd = new Random();
                int chosenIndex = rnd.Next(_descriptonsList.Count - 1);
                _ChosenSet.Add(_descriptonsList.ElementAt(chosenIndex),_numbersList.ElementAt(chosenIndex));
                _numbersList.RemoveAt(chosenIndex);
                _descriptonsList.RemoveAt(chosenIndex);
            }
            
        }
        
        
        private void PopulateDescriptions()
        {
            _descriptonsList.Add("General Knowledge");
            _descriptonsList.Add("Philosophy");
            _descriptonsList.Add("Psychology");
            _descriptonsList.Add("Religion");
            _descriptonsList.Add("Social Sciences");
            _descriptonsList.Add("Languages");
            _descriptonsList.Add("Science");
            _descriptonsList.Add("Technology");
            _descriptonsList.Add("Arts");
            _descriptonsList.Add("Recreation");
            _descriptonsList.Add("Literature");
            _descriptonsList.Add("History");
            _descriptonsList.Add("Geography");
        }

        private void PopulateNumbers()
        {
            _numbersList.Add(0);
            _numbersList.Add(100);
            _numbersList.Add(100);
            _numbersList.Add(200);
            _numbersList.Add(300);
            _numbersList.Add(400);
            _numbersList.Add(500);
            _numbersList.Add(600);
            _numbersList.Add(700);
            _numbersList.Add(700);
            _numbersList.Add(800);
            _numbersList.Add(900);
            _numbersList.Add(900);
        }

        public bool CheckAnswerString(String input,IDictionary<string, int> _set  )
        {

            return true;
        }
        
        public bool CheckAnswerNumber(double input,IDictionary<string, int> _set,int answerLocation )
        {
            double changed = Math.Floor(input);
            int workWith = (int)Math.Round(changed - 50);
            int rounded = ((workWith + 99) / 100 ) * 100;
            
            Debug.WriteLine("Comparing " + rounded + " vs " + _set.Values.ElementAt(answerLocation));
            if (rounded == _set.Values.ElementAt(answerLocation))
            {
                return true;
            }
            return false;
        }
    }
}