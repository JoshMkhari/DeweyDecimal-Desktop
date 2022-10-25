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

        public static int setCount;
        public QuestionsAnswersModel(List<double> numbers, int mode)
        {
            _descriptonsList = new List<string>();
            _numbersList = new List<int>();
            _ChosenSet = new Dictionary<string, int>();
            
            PopulateDescriptions();
            PopulateNumbers();
            PopulateChosenSet(numbers, mode);

        }

        private void PopulateChosenSet(List<double> numbers,int mode)
        {
            IDictionary<string, int> Set = new Dictionary<string, int>();
            for (int i = 0; i < 4; i++)
            {
                double changed = Math.Floor(numbers.ElementAt(i));
                int rounded = (((int)changed) / 100 ) * 100;
                for (int j = 0; j < _numbersList.Count; j++)
                {
                    if (rounded == _numbersList.ElementAt(j))
                    {
                        Set.Add(_descriptonsList.ElementAt(j),_numbersList.ElementAt(j));
                        _numbersList.RemoveAt(j);
                        _descriptonsList.RemoveAt(j);
                    }
                }
                
            }
            Debug.WriteLine("2 Set count " + Set.Count);
            int repeatSize = 4;
            if (mode == 0)
            {
                repeatSize = 7;
                int repeat = 3;

                while (Set.Count+repeat!=7)
                {
                    repeat++;
                }
                for (int i = 0; i < repeat; i++)
                {
                    var rnd = new Random();
                    int chosenIndex = rnd.Next(_descriptonsList.Count - 1);
                    Set.Add(_descriptonsList.ElementAt(chosenIndex),_numbersList.ElementAt(chosenIndex));
                    _numbersList.RemoveAt(chosenIndex);
                    _descriptonsList.RemoveAt(chosenIndex);
                }
            }

            if (Set.Count ==4 || Set.Count == 7)
            {
                for (int i = 0; i < repeatSize; i++)
                {
                    var rnd = new Random();
                    Debug.WriteLine("3 Set count " + Set.Count);
                    int chosenIndex = rnd.Next(Set.Count - 1);
                    _ChosenSet.Add(Set.ElementAt(chosenIndex));
                    Set.Remove(Set.Keys.ElementAt(chosenIndex));
                }
                setCount = repeatSize;
            }
            else
            {
                setCount = 1;
            }
            
        }
        
        
        private void PopulateDescriptions()
        {
            _descriptonsList.Add("General Knowledge");
            _descriptonsList.Add("Philosophy/Psych");
            _descriptonsList.Add("Religion");
            _descriptonsList.Add("Social Sciences");
            _descriptonsList.Add("Languages");
            _descriptonsList.Add("Science");
            _descriptonsList.Add("Technology");
            _descriptonsList.Add("Arts/Recreation");
            _descriptonsList.Add("Literature");
            _descriptonsList.Add("History/Geography");
        }

        private void PopulateNumbers()
        {
            _numbersList.Add(0);
            _numbersList.Add(100);
            _numbersList.Add(200);
            _numbersList.Add(300);
            _numbersList.Add(400);
            _numbersList.Add(500);
            _numbersList.Add(600);
            _numbersList.Add(700);
            _numbersList.Add(800);
            _numbersList.Add(900);
        }

        public bool CheckAnswerString(KeyValuePair<string, int> set,double answerPair )
        {
            //Debug.WriteLine("ChosenSetKey at elemtnet " + _questionsAnswersModel._ChosenSet.Keys.ElementAt(_textBlockNum));
            //Debug.WriteLine("ChosenSetKey at elemtnet " + _replaceBooksViewModel.CallNumbers.ElementAt(hexNum));
            
            Debug.WriteLine("Set key " + set.Key);
            Debug.WriteLine("Set Value " + set.Value);
            
            double changed = Math.Floor(answerPair);
            int rounded = (((int)changed) / 100 ) * 100;
            Debug.WriteLine("AnswerPair " + rounded);
            if (rounded == set.Value)
            {
                return true;
            }
            return false;
        }
        
        public bool CheckAnswerNumber(double input,IDictionary<string, int> set,int answerLocation )
        {
            double changed = Math.Floor(input);
            int rounded = (((int)changed) / 100 ) * 100;
            if (rounded == set.Values.ElementAt(answerLocation))
            {
                return true;
            }
            return false;
        }
    }
}