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
        public Path hex;

        private ColoursModel _coloursModel;
        public HexagonModel()
        {
            int[] leftValues = new []{ 12,112,212,306,404,502,594};
            int[] topValues = new []{ 0,104,0,104,0,104,0 };
            
            HexDefaults = new int[2][];
            HexDefaults[0] = leftValues;
            HexDefaults[1] = topValues;
            
            HexagonList = new List<Path>();
            _coloursModel = new ColoursModel();
            CreateHexagons();
        }

        private void CreateHexagons()
        {
            for (int i = 0; i < 7; i++)
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
                Canvas.SetTop(currentHex,HexDefaults[1][i]);
                Panel.SetZIndex(currentHex,5);
                HexagonList.Add(currentHex);
            }
            
            
        }
    }
}