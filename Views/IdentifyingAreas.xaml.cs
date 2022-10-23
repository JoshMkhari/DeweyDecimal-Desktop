using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using JoshMkhariPROG7312Game.Logic.Identifying_Areas;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;

namespace JoshMkhariPROG7312Game.Views
{
    public partial class IdentifyingAreas : UserControl
    {
        private int _ballX = 0;
        private int _ballY = 0;
        private int _missed = 0;
        private int _scored = 0;
        private int _currentRound = 0;
        
        
        public IdentifyingAreas()
        {
            InitializeComponent();
            //https://stackoverflow.com/questions/11485843/how-can-i-create-hexagon-menu-using-wpf

            HexagonModel hexagonModel = new HexagonModel();
            
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Path hex in hexagonModel.HexagonList)
            {
                hex.MouseLeftButtonDown += OnHexClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                IdentifyAreaCanvas.Children.Add(hex);
                
            }
            
            BasketBallModel basketBallModel = new BasketBallModel();
            
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Image currentBall in basketBallModel.BallLocationList)
            {
                currentBall.MouseLeftButtonDown += OnBallClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                IdentifyAreaCanvas.Children.Add(currentBall);
                
            }

            //BorderModel borderModel = new BorderModel(1);
            //borderModel.AssignValuesToBlocks();
            
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,1);
            dispatcherTimer.Start();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
        }
        
        private void OnHexClick(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/67609123/wpf-c-sharp-create-click-event-for-dynamically-created-button
            Path currentHex = (Path)sender;
            string name = currentHex.Name;
            Debug.WriteLine(name);
            switch (name)
            {
                case "Hex0":
                {
                    Debug.WriteLine("Mama i made it");
                    break;
                }
            }
        }
        
        private void OnBallClick(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/67609123/wpf-c-sharp-create-click-event-for-dynamically-created-button
            Image currentBall = (Image)sender;
            string name = currentBall.Name;
            Debug.WriteLine(name);
            switch (name)
            {
                case "Hex0":
                {
                    Debug.WriteLine("Mama i made it");
                    break;
                }
            }
        }
    }
}