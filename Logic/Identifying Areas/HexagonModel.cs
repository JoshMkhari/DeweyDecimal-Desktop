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
        private int[][] HexDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        private byte[][] BorderDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        
        public List<Path> HexagonList { get; set; }

        public Path hex;
        public HexagonModel()
        {
            int[] leftValues = new []{ 11,111,212,306,403,501,593};
            int[] topValues = new []{ 0,104,0,104,0,108,0 };
            
            HexDefaults = new int[2][];
            HexDefaults[0] = leftValues;
            HexDefaults[1] = topValues;
            
            
            byte[] borderReds = {108,240, 176,112,254,129,39,181,30,88};
            byte[] borderGreen = {71,145, 198,168,202,129,51,71,136,51};
            byte[] borderBlues= { 34,60,83,188,80,129,73,106,109,84};
            
            BorderDefaults = new byte[3][];
            BorderDefaults[0] = borderReds;
            BorderDefaults[1] = borderGreen;
            BorderDefaults[2] = borderBlues;

            HexagonList = new List<Path>();
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
                    RenderTransformOrigin = new Point(0.5,0.5) ,
                    RenderTransform = rt,
                    Stretch = Stretch.Uniform, //https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.stretch?view=windowsdesktop-6.0
                    Fill = new SolidColorBrush(Color.FromRgb(BorderDefaults[0][i], BorderDefaults[1][i],
                        BorderDefaults[2][i])), //https://www.rapidtables.com/convert/color/hex-to-rgb.html
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