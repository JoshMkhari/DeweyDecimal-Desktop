using System;
using System.Collections.Generic;
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
using JoshMkhariPROG7312Game.ViewModels;

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

        private int _textBlockNum;

        private int _numBasketBallsAvailable;

        private int _gameMode;
        private Point _ballDestination,_ballStartLocation;

        private bool _aimSet, _ballChosen, _targetReached, _ballStop;
        private Image _currentBall;
        private Path _destination;
        private DispatcherTimer _ballTimer, _gaugeTimer;

        private QuestionsAnswersModel _questionsAnswersModel;
        private BorderModel _borderModel;
        private ReplaceBooksViewModel _replaceBooksViewModel;
        private BasketBallModel _basketBallModel;
        private HexagonModel _hexagonModel;
        public IdentifyingAreas()
        {
            InitializeComponent();
            //https://stackoverflow.com/questions/11485843/how-can-i-create-hexagon-menu-using-wpf

            _gameMode = 0;
            _numBasketBallsAvailable = 4; 
            _hexagonModel = new HexagonModel();
            _aimSet = false;
            _ballChosen = false;
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Path hex in _hexagonModel.HexagonList)
            {
                hex.MouseLeftButtonDown += OnHexClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                IdentifyAreaCanvas.Children.Add(hex);
                
            }
            
            _basketBallModel = new BasketBallModel();
            
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Image currentBall in _basketBallModel.BallLocationList)
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

            
           
            //_questionsAnswersModel._ChosenSet;
 
            
            CreateQuestionsAnswers();
            
            

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

            if (Canvas.GetLeft(_currentBall) == _ballDestination.X 
                && Canvas.GetTop(_currentBall) == _ballDestination.Y)
            {
                //To send ball back down through
                Panel.SetZIndex(_currentBall,7);
                if (_ballStop)
                {
                    StopBall();
                }
                else
                {
                    _ballDestination.Y += 50;
                    _ballStop = true;
                }
            }

            if (Canvas.GetLeft(_currentBall) < 0
                || Canvas.GetLeft(_currentBall)> 750
                || Canvas.GetTop(_currentBall) < 0
                || Canvas.GetTop(_currentBall) >500)
            {
                StopBall();
            }
            //Move ball down through net
            if (Canvas.GetTop(_currentBall)<_ballDestination.Y)
            {
                
                Canvas.SetTop(_currentBall,Canvas.GetTop(_currentBall)+2);
            }
        }
        
        
        private void StopBall()
        {
            _ballTimer.Stop();
            _aimSet = false;
            _ballChosen = false;
            _ballStop = false;
            Canvas.SetTop(indicatorLevel,424);
            Panel.SetZIndex(_currentBall,11);
            Canvas.SetLeft(_currentBall,_ballStartLocation.X);
            Canvas.SetTop(_currentBall,_ballStartLocation.Y);
            if (_targetReached)
            {
                //Check ball number to see which text block it belongs to
                if (IsCorrectAnswer())
                {
                    _scored++;
                    _currentBall.Visibility = Visibility.Collapsed;
                    _borderModel.CallBlockBordersList.ElementAt(_textBlockNum).Visibility = Visibility.Collapsed;
                    _numBasketBallsAvailable--;
                    
                }
                else
                {
                    _scored--;
                }
                TxtScoredCount.Content = _scored;
                if (_numBasketBallsAvailable == 0)
                {
                    ChangeMode();
                }
            }
            else
            {
                _missed++;
                TxtMissedCount.Content = _missed;
            }
            
        }

        private void ChangeMode()
        {
            if (_gameMode == 0)
            {
                _gameMode++;
            }
            else
            {
                _gameMode = 0;
            }

            foreach (Image currentBall in _basketBallModel.BallLocationList)
            {
                currentBall.Visibility = Visibility.Visible;
            }
            
            //Remove CallBlocks
            foreach (Image currentBall in _basketBallModel.BallLocationList)
            {
                currentBall.Visibility = Visibility.Visible;
            }
            foreach (Border currentBorder in _borderModel.CallBlockBordersList)
            {
                IdentifyAreaCanvas.Children.Remove(currentBorder);
                
            }
            foreach (Border currentBorder in _borderModel.AnswerBlockBordersList)
            {
                IdentifyAreaCanvas.Children.Remove(currentBorder);
            }

            CreateQuestionsAnswers();
        }

        private void CreateQuestionsAnswers()
        {
                       
            _replaceBooksViewModel = new ReplaceBooksViewModel(1); 
            
            if (_gameMode == 0)
            {
                _borderModel = new BorderModel(1);
                _questionsAnswersModel = new QuestionsAnswersModel(_replaceBooksViewModel.CallNumbers,_gameMode);
                List<string> texts = new List<string>();
                for (int i = 0; i < _questionsAnswersModel._ChosenSet.Count; i++)
                {
                    texts.Add(_questionsAnswersModel._ChosenSet.Keys.ElementAt(i));
                }
                _borderModel.AssignValuesToBlocks(_replaceBooksViewModel.CallNumbers,texts ,4,0, new HexagonModel(),1);

                foreach (Border currentBorder in _borderModel.CallBlockBordersList)
                {
                    //currentBall.MouseLeftButtonDown += OnBallClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                    IdentifyAreaCanvas.Children.Add(currentBorder);
                
                }
                _borderModel.CreateQuestionBlocks(_questionsAnswersModel,_gameMode,_hexagonModel,_replaceBooksViewModel.CallNumbers,_replaceBooksViewModel.CallNumbersStrings);
                foreach (Border currentBorder in _borderModel.AnswerBlockBordersList)
                {
                    //currentBall.MouseLeftButtonDown += OnBallClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                    IdentifyAreaCanvas.Children.Add(currentBorder);
                
                }   
            }
            else
            {
                _borderModel = new BorderModel(2);
                
                _borderModel.AssignValuesToBlocks(_replaceBooksViewModel.CallNumbers,_replaceBooksViewModel.CallNumbersStrings,7,0, _hexagonModel,2);
                
                foreach (Border currentBorder in _borderModel.CallBlockBordersList)
                {
                    //currentBall.MouseLeftButtonDown += OnBallClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                    IdentifyAreaCanvas.Children.Add(currentBorder);
                }
                
                _questionsAnswersModel = new QuestionsAnswersModel(_replaceBooksViewModel.CallNumbers,_gameMode);
                _borderModel.CreateQuestionBlocks(_questionsAnswersModel,_gameMode,_hexagonModel, new List<double>(),new List<string>());
                foreach (Border currentBorder in _borderModel.AnswerBlockBordersList)
                {
                    //currentBall.MouseLeftButtonDown += OnBallClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                    IdentifyAreaCanvas.Children.Add(currentBorder);
                
                }   
            }

        }

        private bool IsCorrectAnswer()
        {
            int hexNum = Convert.ToInt32(_destination.Name.Substring(3)); 

            if (_gameMode == 0)//Basketballs have callnumbers beneath them
            {
               return _questionsAnswersModel.CheckAnswerNumber(_replaceBooksViewModel.CallNumbers.ElementAt(_textBlockNum),
                    _questionsAnswersModel._ChosenSet,hexNum);
            }

            //return _questionsAnswersModel.CheckAnswerString(_replaceBooksViewModel.CallNumbers.ElementAt(_textBlockNum),
                //_questionsAnswersModel._ChosenSet);
                return false;

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
            _ballStartLocation = new Point(Canvas.GetLeft(_currentBall), Canvas.GetTop(_currentBall));
            _ballChosen = true;
        }

        private void IdentifyAreaCanvas_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_aimSet|| !_ballChosen) return;
            
            if (e.Key != Key.Space) return;

            _textBlockNum = Convert.ToInt32(_currentBall.Name.Substring(3));
            //Debug.WriteLine("From text block " + _textBlockNum);
            _gaugeTimer.Stop();
            SetTargetAccuracy(Canvas.GetTop(indicatorLevel));
        }

        private void SetTargetAccuracy(double gauge)
        {
            var rnd = new Random();

            int updateAccuracy = 0;

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
            _targetReached = true;
            if (updateAccuracy != 0)
            {
                _targetReached = false;
                int both = rnd.Next(1, 10);
                if (both % 2 == 0)
                {
                    _ballDestination.X += MakeEven(rnd.Next(20, updateAccuracy));
                    _ballDestination.Y += MakeEven(rnd.Next(20, updateAccuracy));
                }
                else
                {
                    int one = rnd.Next(1, 2);
                    if (one == 1)
                    {
                        _ballDestination.X += MakeEven(rnd.Next(20, updateAccuracy));
                    }
                    else
                    {
                        _ballDestination.Y += MakeEven(rnd.Next(20, updateAccuracy)); 
                    }
                }
            }
            
            _ballTimer.Interval = new TimeSpan(0,0,0,0,1);
            _ballTimer.Start();
        }

        private int MakeEven(int num)
        {
            if (num % 2 == 0) return num;
            num++;
            return num;

        }
    }
}