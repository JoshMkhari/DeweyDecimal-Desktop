using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class RectangleModel
    {
        public RectangleModel()
        {
            int[] leftValues = { 314, 314, 167, 470 };
            int[] topValues = { 82, 308, 205, 205 };

            RectangleDefaults = new int[2][];
            RectangleDefaults[0] = leftValues;
            RectangleDefaults[1] = topValues;

            SelectRectanglesList = new List<Rectangle>();
            CreateSelectionRectangles();
        }

        private int[][] RectangleDefaults { get; } //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        public List<Rectangle> SelectRectanglesList { get; set; }

        private void CreateSelectionRectangles()
        {
            for (var i = 0; i < 4; i++)
            {
                var currentRectangle = new Rectangle();
                Panel.SetZIndex(currentRectangle, 20);
                currentRectangle.Width = 94;
                currentRectangle.Height = 146;
                currentRectangle.Fill = new SolidColorBrush(Colors.Transparent);
                Canvas.SetLeft(currentRectangle, RectangleDefaults[0][i]);
                Canvas.SetTop(currentRectangle, RectangleDefaults[1][i]);
                SelectRectanglesList.Add(currentRectangle);
            }
        }
    }
}