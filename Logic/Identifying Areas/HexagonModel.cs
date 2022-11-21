using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JoshMkhariPROG7312Game.Logic.Identifying_Areas
{
    public class HexagonModel
    {
        private int[][] HexDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays

        public List<Path> HexagonList { get; set; }
        public List<TextBlock> TextList { get; set; }

        private ColoursModel _coloursModel;
        public HexagonModel(int mode)
        {
            int[] leftValues;
            int[] topValues;
            if (mode == 1)
            {
                leftValues  = new []{ 12,112,212,306,404,502,594};  
                topValues = new []{ 0,104,0,104,0,104,0 };
            }
            else
            {
                leftValues = new []{ 90,250,400,553};
                topValues = new []{ 265,285,305,325 };
            }
            

            
            HexDefaults = new int[2][];
            HexDefaults[0] = leftValues;
            HexDefaults[1] = topValues;
            
            HexagonList = new List<Path>();
            TextList = new List<TextBlock>();
            _coloursModel = new ColoursModel();
            CreateHexagons(mode);
        }

        private void CreateHexagons(int mode)
        {
            int nums = 4;

            if (mode == 1)
            {
                nums = 7;
            }
            for (int i = 0; i < nums; i++)
            {
                RotateTransform rt = new RotateTransform(-90);
                Path currentHex = new Path
                {
                    Width = 120,
                    Height = 120,
                    Name = "Hex"+i,
                    RenderTransformOrigin = new Point(0.5,0.5) ,
                    RenderTransform = rt,//https://www.c-sharpcorner.com/uploadfile/mahesh/rotatetransform-in-wpf/
                    Stretch = Stretch.Uniform, //https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.stretch?view=windowsdesktop-6.0
                    Fill = new SolidColorBrush(Color.FromRgb(_coloursModel.ColourDefaults[0][i], _coloursModel.ColourDefaults[1][i],
                        _coloursModel.ColourDefaults[2][i])), //https://www.rapidtables.com/convert/color/hex-to-rgb.html
                    Data = Geometry.Parse("M8.660254,0 L17.320508,5 17.320508,15 8.660254,20 0,15 0,5 8.660254,0 Z")
                };
                Canvas.SetLeft(currentHex,HexDefaults[0][i]);
                if (mode == 1)
                {
                    Canvas.SetTop(currentHex,HexDefaults[1][i]);
                }
                else
                {
                    Debug.WriteLine("we in 2");
                    TextBlock currenText = new TextBlock
                    {
                        Name = "Text"+i,
                        Text = "myMaN",
                        Foreground = Brushes.Black,
                        FontSize = 10
                    };
                    Canvas.SetLeft(currenText,HexDefaults[0][i]);
                    Canvas.SetTop(currentHex,265);
                    Canvas.SetTop(currenText,HexDefaults[1][i]);
                    TextList.Add(currenText);
                }
                
                Panel.SetZIndex(currentHex,0);
                HexagonList.Add(currentHex);
                
            }
        }
    }
}