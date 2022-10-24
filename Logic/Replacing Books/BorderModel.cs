using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using JoshMkhariPROG7312Game.Logic.Identifying_Areas;
using JoshMkhariPROG7312Game.ViewModels;
using JoshMkhariPROG7312Game.Views;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class BorderModel
    {
        private double BorderInitialLeft { get;}
        private int[] BorderInitialTop { get; }
        public List<Border> CallBlockBordersList { get; }
        public List<Border> AnswerBlockBordersList { get; }
        private ColoursModel _coloursModel;
        public BorderModel(int mode)
        {
            CallBlockBordersList = new List<Border>();
            _coloursModel = new ColoursModel();
            switch (mode)
            {
                case 0://Replacing books model
                {
                    BorderInitialLeft = 321;

                    BorderInitialTop = new []{ 422, 198 };
                    //https://www.rapidtables.com/convert/color/hex-to-rgb.html
                    break;
                }
                default:
                {
                    BorderInitialLeft = 185;
                    AnswerBlockBordersList = new List<Border>();
                    break;
                }
            }

        }

        public void CreateQuestionBlocks(QuestionsAnswersModel questionsAnswersModel, int mode, HexagonModel hexagonModel,List<double> numbers, List<string> texts)
        {
            if (mode == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Border rectBlock = new Border
                    {
                        //rectBlock.Name = "border" + i;
                        BorderThickness = new Thickness(2),
                        BorderBrush =Brushes.White,
                        Width = 80,
                        Height = 22
                        //Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)) //https://www.rapidtables.com/convert/color/hex-to-rgb.html
                    };

                    TextBlock textForBlock = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = Brushes.White,
                        Text = questionsAnswersModel._ChosenSet.Keys.ElementAt(i)
                    };
                    rectBlock.Child = textForBlock;
                    Panel.SetZIndex(rectBlock,6);
                    AnswerBlockBordersList.Add(rectBlock);
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    Border rectBlock = new Border
                    {
                        //rectBlock.Name = "border" + i;
                        BorderThickness = new Thickness(2),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        Width = 80,
                        Height = 22,
                        Background = new SolidColorBrush(Color.FromRgb(_coloursModel.ColourDefaults[0][i], _coloursModel.ColourDefaults[1][i], _coloursModel.ColourDefaults[2][i])) //https://www.rapidtables.com/convert/color/hex-to-rgb.html
                    };

                    TextBlock textForBlock = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = NumberFormatter(numbers.ElementAt(i)) + 
                               texts.ElementAt(i)
                    };
                    rectBlock.Child = textForBlock;
                    CallBlockBordersList.Add(rectBlock);
                }
            }

            PlaceQuestionsAtPositions(hexagonModel, mode); 
        }

        private void PlaceQuestionsAtPositions(HexagonModel hexagonModel,int mode)
        {
            
            //Those center positions the ball uses for targets
            if (mode == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Canvas.SetLeft(CallBlockBordersList.ElementAt(i),BorderInitialLeft+(i*113));
                    Canvas.SetTop(CallBlockBordersList.ElementAt(i),465);
                } 
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {

                    Canvas.SetLeft(AnswerBlockBordersList.ElementAt(i),Canvas.GetLeft(hexagonModel.HexagonList.ElementAt(i))+20);
                    Canvas.SetTop(AnswerBlockBordersList.ElementAt(i),Canvas.GetTop(hexagonModel.HexagonList.ElementAt(i))+40);
                } 
            }

        }
        public void AssignValuesToBlocks(List<double> numbers, List<string> texts, int numItems, int start,HexagonModel hexagonModel, int mode)
        {
            if (mode == 0)
            {
                for (int i = start; i < numItems; i++)
                {
                    Border rectBlock = new Border
                    {
                        //rectBlock.Name = "border" + i;
                        BorderThickness = new Thickness(2),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        Width = 80,
                        Height = 22,
                        Background = new SolidColorBrush(Color.FromRgb(_coloursModel.ColourDefaults[0][i], _coloursModel.ColourDefaults[1][i], _coloursModel.ColourDefaults[2][i])) //https://www.rapidtables.com/convert/color/hex-to-rgb.html
                    };

                    TextBlock textForBlock = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = NumberFormatter(numbers.ElementAt(i)) + 
                               texts.ElementAt(i)
                    };
                    rectBlock.Child = textForBlock;
                    CallBlockBordersList.Add(rectBlock);
                }
            }
            else
            {
                for (int i = start; i < numItems; i++)
                {
                    Border rectBlock = new Border
                    {
                        //rectBlock.Name = "border" + i;
                        BorderThickness = new Thickness(2),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        Width = 80,
                        Height = 22,
                        Background = new SolidColorBrush(Color.FromRgb(_coloursModel.ColourDefaults[0][i], _coloursModel.ColourDefaults[1][i], _coloursModel.ColourDefaults[2][i])) //https://www.rapidtables.com/convert/color/hex-to-rgb.html
                    };

                    TextBlock textForBlock = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = Brushes.White,
                        Text = texts.ElementAt(i)
                    };
                    rectBlock.Child = textForBlock;
                    CallBlockBordersList.Add(rectBlock);
                }
            }
      
            PlaceBlocksAtStartPositions(hexagonModel, mode); 
        }

        public void PlaceBlocksAtStartPositions(HexagonModel hexagonModel, int currentMode)
        {
            Debug.WriteLine("This is current modfe " + currentMode);
            switch (currentMode)
            {
                case 0:
                {
                    for (int i = 0; i < 10; i++)
                    {
                
                        Canvas.SetLeft(CallBlockBordersList.ElementAt(i),BorderInitialLeft);
                        if (i < 5)
                        {
                            Canvas.SetTop(CallBlockBordersList.ElementAt(i),BorderInitialTop[0]-(i*27));  
                        }
                        else
                        {
                            int current = i - 5;
                            Canvas.SetTop(CallBlockBordersList.ElementAt(i),BorderInitialTop[1]-(current*27));
                        }
                    } 
                    break;
                }
                case 1:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Canvas.SetLeft(CallBlockBordersList.ElementAt(i),BorderInitialLeft+(i*113));
                        Canvas.SetTop(CallBlockBordersList.ElementAt(i),465);
                    }
                    break;
                }
                case 2:
                {
                    for (int i = 0; i < 7; i++)
                    {
                        Canvas.SetLeft(CallBlockBordersList.ElementAt(i),Canvas.GetLeft(hexagonModel.HexagonList.ElementAt(i))+20);
                        Canvas.SetTop(CallBlockBordersList.ElementAt(i),Canvas.GetTop(hexagonModel.HexagonList.ElementAt(i))+40);
                    }
                    break;
                }
            }

            
        }
        
        private String NumberFormatter(double input)
        {
            String num = input.ToString();
            
            //Look for comma
            int commaLocation = 0;
            for (int i = 0; i < num.Length; i++)
            {
                if (num.Substring(i, 1).Equals(","))
                {
                    //Comma found
                    commaLocation = i;
                }
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(num);
            while (commaLocation!=3)
            {
                stringBuilder.Insert(0, "0");//https://www.softwaretestinghelp.com/csharp-stringbuilder/#:~:text=C%23%20StringBuilder%20Methods%201%20%231%29%20Append%20Method%20As,Replace%20Method%20...%206%20%236%29%20Equals%20Method%20
                commaLocation++;
            }
            
            return stringBuilder.ToString();
        }
    }
}