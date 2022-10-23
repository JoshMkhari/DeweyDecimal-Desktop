using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using JoshMkhariPROG7312Game.Logic.Identifying_Areas;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;

namespace JoshMkhariPROG7312Game.Views
{
    public partial class IdentifyingAreas : UserControl
    {
        //ball speed
        private int _ballX = 0;
        private int _ballY = 0;
        
        private int _missed = 0;
        private int _scored = 0;
        private int _currentRound = 0;

        private Point _ballDestination;

        private bool _aimSet, _ballChosen;
        private Image _currentBall;
        private Path _destination;
        private System.Windows.Threading.DispatcherTimer _ballTimer;
        public IdentifyingAreas()
        {
            InitializeComponent();
            IdentifyAreaCanvas.Focus();
            //https://stackoverflow.com/questions/11485843/how-can-i-create-hexagon-menu-using-wpf

            HexagonModel hexagonModel = new HexagonModel();
            _aimSet = false;
            _ballChosen = false;
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
            
            _ballTimer = new System.Windows.Threading.DispatcherTimer();
            _ballTimer.Tick += ballTimer_Tick;

        }

        private void ballTimer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine(Canvas.GetLeft(_currentBall));
            Debug.WriteLine(Canvas.GetLeft(_destination));
            //If ball is to right of target
            if (Canvas.GetLeft(_currentBall)>Canvas.GetLeft(_destination))
            {
                Canvas.SetLeft(_currentBall,Canvas.GetLeft(_currentBall)-2);
            }
            
            //If ball is to left of target
            if (Canvas.GetLeft(_currentBall)<Canvas.GetLeft(_destination))
            {
                Canvas.SetLeft(_currentBall,Canvas.GetLeft(_currentBall)+2);
            }
            
            //Move ball up towards target
            if (Canvas.GetTop(_currentBall)>Canvas.GetTop(_destination))
            {
                
                Canvas.SetTop(_currentBall,Canvas.GetTop(_currentBall)-2);
            }
           

            if (Canvas.GetLeft(_currentBall) == Canvas.GetLeft(_destination) && Canvas.GetTop(_currentBall) == Canvas.GetTop(_destination))
            {
                _aimSet = false;
                _ballChosen = false;
                _ballTimer.Stop();
            }
        }
        
        private void OnHexClick(object sender, RoutedEventArgs e)
        {
            
            if(_aimSet || !_ballChosen){return;}//Prevent user from double clicking same target
            _aimSet = true;
            _ballTimer.Interval = new TimeSpan(0,0,0,0,1);
            _ballTimer.Start();
            
            
            //https://stackoverflow.com/questions/67609123/wpf-c-sharp-create-click-event-for-dynamically-created-button
            Path currentHex = (Path)sender;
            _destination = currentHex;
            string name = currentHex.Name;
            
        }
        
        private void OnBallClick(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/67609123/wpf-c-sharp-create-click-event-for-dynamically-created-button
            if(_ballChosen){return;}
            Image currentBall = (Image)sender;
            string name = currentBall.Name;
            Debug.WriteLine(name + " clicked");
            _currentBall = currentBall;
            _ballChosen = true;
        }

        private void IdentifyAreaCanvas_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                //Stop Guage and store guage value
            }
        }
        
    }
}