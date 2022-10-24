using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
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

        private int _gaugeSpeed = 2;
        private Point _ballDestination;

        private bool _aimSet, _ballChosen;
        private Image _currentBall;
        private Path _destination;
        private DispatcherTimer _ballTimer, _gaugeTimer;
        
        public IdentifyingAreas()
        {
            InitializeComponent();
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

            RimNetModel rimNetModel = new RimNetModel();
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Image currentRim in rimNetModel.NetLocationList)
            {
                //currentBall.MouseLeftButtonDown += OnBallClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                IdentifyAreaCanvas.Children.Add(currentRim);
                
            }
            

            //BorderModel borderModel = new BorderModel(1);
            //borderModel.AssignValuesToBlocks();
            
            _ballTimer = new DispatcherTimer();
            _ballTimer.Tick += ballTimer_Tick;

            _gaugeTimer = new DispatcherTimer();
            _gaugeTimer.Tick += gaugeTimer_Tick;

        }

        private void gaugeTimer_Tick(object sender, EventArgs e)
        {
            //Move Indicator up and back down
            Canvas.SetTop(indicatorLevel,Canvas.GetTop(indicatorLevel)-_gaugeSpeed);

            if (Canvas.GetTop(indicatorLevel)<236 || Canvas.GetTop(indicatorLevel)>424)
            {
                _gaugeSpeed = -_gaugeSpeed;
            }
        }
        private void ballTimer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine(Canvas.GetLeft(_currentBall) + " currentBall Left");
            Debug.WriteLine(Canvas.GetTop(_currentBall)+ " currentBall Top");

            //If ball is to right of target
            if (Canvas.GetLeft(_currentBall)>_ballDestination.X)
            {
                Canvas.SetLeft(_currentBall,Canvas.GetLeft(_currentBall)-2);
            }
            
            //If ball is to left of target
            if (Canvas.GetLeft(_currentBall)<_ballDestination.X)
            {
                Canvas.SetLeft(_currentBall,Canvas.GetLeft(_currentBall)+2);
            }
            
            //Move ball up towards target
            if (Canvas.GetTop(_currentBall)>_ballDestination.Y)
            {
                
                Canvas.SetTop(_currentBall,Canvas.GetTop(_currentBall)-2);
            }

            if (Canvas.GetLeft(_currentBall) == _ballDestination.X && Canvas.GetTop(_currentBall) == _ballDestination.Y)
            {
                _aimSet = false;
                _ballChosen = false;
                _ballTimer.Stop();
                
                //To send ball back down through
                Panel.SetZIndex(_currentBall,7);
            }

            if (Canvas.GetLeft(_currentBall) < 0
                || Canvas.GetLeft(_currentBall)> 750
                || Canvas.GetTop(_currentBall) < 0
                || Canvas.GetTop(_currentBall) >500)
            {
                _aimSet = false;
                _ballChosen = false;
                _ballTimer.Stop();
                
                //To send ball back down through
                Panel.SetZIndex(_currentBall,7);
            }
        }
        
        private void OnHexClick(object sender, RoutedEventArgs e)
        {
            
            if(_aimSet || !_ballChosen){return;}//Prevent user from double clicking same target
            _aimSet = true;
            
            
            
            //https://stackoverflow.com/questions/67609123/wpf-c-sharp-create-click-event-for-dynamically-created-button
            Path currentHex = (Path)sender;
            _destination = currentHex;
            double left =Canvas.GetLeft(currentHex) + 30;

            double top =Canvas.GetTop(currentHex) + 30;

            string name = currentHex.Name;
            _ballDestination = new Point(left, top);

            _gaugeTimer.Interval = new TimeSpan(0,0,0,0,1);
            _gaugeTimer.Start();
            IdentifyAreaCanvas.Focus();
        }
        
        private void OnBallClick(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/67609123/wpf-c-sharp-create-click-event-for-dynamically-created-button
            if(_ballChosen){return;}
            Image currentBall = (Image)sender;
            
            string name = currentBall.Name;
            _currentBall = currentBall;
            _ballChosen = true;
        }

        private void IdentifyAreaCanvas_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                //Stop Gauge and store gauge value
                _gaugeTimer.Stop();
                SetTargetAccuracy(Canvas.GetTop(indicatorLevel));

            }
        }

        private void SetTargetAccuracy(double gauge)
        {
            //236
            //424
            Debug.WriteLine(" Ball destination X at start " + _ballDestination.X);
            Debug.WriteLine(" Ball destination Y at start " + _ballDestination.Y);
            Debug.WriteLine(" Guage value " + gauge);
            var rnd = new Random();

            
            
            
            int updateAccuracy = 0;
            
            
            if (gauge < 250)
            {
                _scored++;
            }
            
            if (gauge < 300 && gauge > 250)
            {
                updateAccuracy = 20;
            }
            if (gauge > 300 && gauge < 350 )
            {
                updateAccuracy = 40;
            }
            if (gauge > 350 && gauge < 400)
            {
                updateAccuracy = 60;
            }
            if (gauge > 400 )
            {
                updateAccuracy = 100;
            }

            if (updateAccuracy != 0)
            {
                int both = rnd.Next(1, 10);
                if (both % 2 == 0)
                {
                    _ballDestination.X += rnd.Next(20, updateAccuracy);
                    _ballDestination.Y += rnd.Next(20, updateAccuracy);
                }
                else
                {
                    int one = rnd.Next(1, 2);
                    if (one == 1)
                    {
                        _ballDestination.X += rnd.Next(20, updateAccuracy);
                    }
                    else
                    {
                        _ballDestination.Y += rnd.Next(20, updateAccuracy); 
                    }
                }
            }
            
            
            Debug.WriteLine(" Ball destination X at end " + _ballDestination.X);
            Debug.WriteLine(" Ball destination Y at end " + _ballDestination.Y);
            _ballTimer.Interval = new TimeSpan(0,0,0,0,1);
            _ballTimer.Start();
        }
    }
}