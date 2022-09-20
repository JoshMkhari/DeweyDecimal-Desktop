using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class RectangleModel
    {
        public int[][] RectangleDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        public List<Rectangle> _selectRectanglesList { get; set; }
        public RectangleModel()
        {
            int[] leftValues = new []{ 314,314,167,470 };
            int[] topValues = new []{ 82,308,205,205 };
            
            RectangleDefaults = new int[2][];
            RectangleDefaults[0] = leftValues;
            RectangleDefaults[1] = topValues;
            
            _selectRectanglesList = new List<Rectangle>();
            CreateSelectionRectangles();
        }
        
        private void CreateSelectionRectangles()
        {
            for (int i = 0; i < 4; i++)
            {
                Rectangle currentRectangle = new Rectangle();
                Panel.SetZIndex(currentRectangle,20);
                currentRectangle.Width = 94;
                currentRectangle.Height = 146;
                currentRectangle.Fill = new SolidColorBrush(Colors.Transparent);
                Canvas.SetLeft(currentRectangle,RectangleDefaults[0][i]);
                Canvas.SetTop(currentRectangle,RectangleDefaults[1][i]);
                _selectRectanglesList.Add(currentRectangle);
            }
        }
        
        
    }
}