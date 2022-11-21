using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using JoshMkhariPROG7312Game.Logic.FindCallNumbers;
using JoshMkhariPROG7312Game.Logic.Identifying_Areas;

namespace JoshMkhariPROG7312Game.Views
{
    public partial class FindCallNumbers : UserControl
    {
        private HexagonModel _hexagonModel;
        private DeweySystem _deweySystem;
        private List<DeweyObject> _options;
        private DeweyObject _answer;
        private int _myRanNum;
        private bool _boolOnMid;
        public FindCallNumbers()
        {
            InitializeComponent();
            _deweySystem = new DeweySystem();

            _hexagonModel = new HexagonModel(2);
            foreach (Path hex in _hexagonModel.HexagonList)
            {
                hex.MouseLeftButtonDown += OnHexClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                CanvasFindCallNumbers.Children.Add(hex);
                
            }
            
            foreach (TextBlock hex in _hexagonModel.TextList)
            {
                CanvasFindCallNumbers.Children.Add(hex);
            }
            GenerateQuestion();
        }

        private void GenerateQuestion()
        {
            //Generate random number
            _boolOnMid = false;
            Random rand = new Random();
            int randNum; 
            do
            {
                randNum = rand.Next(999);
            } while (randNum%100==0  || randNum%10 == 0);

            //Chosen randomNumber is 678
            DeweyObject deweyObject = _deweySystem.ReturnNodes(randNum);

            if (deweyObject._description.Equals("[Unassigned]") || deweyObject._description.Equals("(Optional number)"))
            {
                GenerateQuestion();
            }
            else
            {
                TextBlockQuestion.Text = deweyObject._description;
                ShowTopLevelAnswers(randNum);
            }
        }

        private void ShowTopLevelAnswers(int num)
        {
            _myRanNum = num;
            _options = new List<DeweyObject>();
            _options.Add( _deweySystem.ReturnTop(num));
            _answer = _options.ElementAt(0);

            List<int> setOfOptions = new List<int>();
            
            setOfOptions.Add(_options.ElementAt(0)._number);
            Random ranOption = new Random();
            for (int i = 0; i < 3; i++)
            {
                int ran = ranOption.Next(_deweySystem._offHun.Count);
                int option = _deweySystem._offHun.ElementAt(ran)._number;
                while (setOfOptions.Contains(option))
                {
                    ran = ranOption.Next(_deweySystem._offHun.Count);
                    option = _deweySystem._offHun.ElementAt(ran)._number;
                }
                setOfOptions.Add(option);
                _options.Add(_deweySystem._offHun.ElementAt(ran));
            }

            ShuffleClass.Shuffle(_options);
            for (int i = 0; i < 4; i++)
            {
                _hexagonModel.TextList.ElementAt(i).Text = _options.ElementAt(i)._number + " " + _options.ElementAt(i)._description;
            }
        }
        
        private void ShowMidLevelAnswers()
        {
            _boolOnMid = true;
            _options = new List<DeweyObject>();
            _options.Add( _deweySystem.ReturnMid(_myRanNum));
            _answer = _options.ElementAt(0);
            List<int> setOfOptions = new List<int>();
            
            setOfOptions.Add(_options.ElementAt(0)._number);
            Random ranOption = new Random();
            for (int i = 0; i < 3; i++)
            {
                int ran = ranOption.Next(_deweySystem._offTen.Count);
                int option = _deweySystem._offTen.ElementAt(ran)._number;
                while (setOfOptions.Contains(option))
                {
                    ran = ranOption.Next(_deweySystem._offTen.Count);
                    option = _deweySystem._offHun.ElementAt(ran)._number;
                }
                setOfOptions.Add(option);
                _options.Add(_deweySystem._offTen.ElementAt(ran));
            }

            ShuffleClass.Shuffle(_options);
            for (int i = 0; i < 4; i++)
            {
                _hexagonModel.TextList.ElementAt(i).Text = _options.ElementAt(i)._number + " " + _options.ElementAt(i)._description;
            }
        }
        private void OnHexClick(object sender, RoutedEventArgs e)
        {
            Path currentHex = (Path)sender;

            int answer;
            switch (currentHex.Name)
            {
                case "Hex0":
                    answer = 0;
                    break;
                case "Hex1":
                    answer = 1;
                    break;
                case "Hex2":
                    answer = 2;
                    break;
                default:
                    answer = 3;
                    break;
            }

            if (_options.ElementAt(answer)._number == _answer._number)
            {
                if (_boolOnMid)
                {
                    GenerateQuestion();
                }
                else
                {
                    ShowMidLevelAnswers();
                }
                
            }
            else
            {
                MessageBox.Show("Wrong");
                GenerateQuestion();
            }
        }
    }
}