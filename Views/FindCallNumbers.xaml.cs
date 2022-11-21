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
        }

        private void GenerateQuestion()
        {
            //Generate random number
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
                TextBlockQuestion.Text = deweyObject._description + "?";
                ShowTopLevelAnswers(randNum);
            }
        }

        private void ShowTopLevelAnswers(int num)
        {
            List<DeweyObject> options = new List<DeweyObject>();
            options.Add( _deweySystem.ReturnTop(num));

            List<int> setOfOptions = new List<int>();
            
            setOfOptions.Add(options.ElementAt(0)._number);
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
                options.Add(_deweySystem._offHun.ElementAt(ran));
            }

            for (int i = 0; i < 4; i++)
            {
                _hexagonModel.TextList.ElementAt(i).Text = options.ElementAt(i)._number + " " + options.ElementAt(i)._description;
            }

        }
        private void OnHexClick(object sender, RoutedEventArgs e)
        {
            GenerateQuestion();

        }
    }
}