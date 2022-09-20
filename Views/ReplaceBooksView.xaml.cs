using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JoshMkhariPROG7312Game.Logic.Replacing_Books;
using JoshMkhariPROG7312Game.ViewModels;
using Border = System.Windows.Controls.Border;

namespace JoshMkhariPROG7312Game.Views
{
    /// <summary>
    ///     Interaction logic for ReplaceBooksView.xaml
    /// </summary>
    public partial class ReplaceBooksView : UserControl
    {
        //Declerations
        private ReplaceBooksViewModel _replaceBooksViewModel;//Used to access all variables and methods within model
        
        private BorderModel _borderModel;
        private LabelsModel _labelModel;
        private ArrowModel _arrowModel;
       // private RectangleModel _rectangleModel;

        private bool _onSettingsPage;
        
        public ReplaceBooksView()
        {
            InitializeComponent();
            _replaceBooksViewModel = new ReplaceBooksViewModel();
            _borderModel = new BorderModel();
            _borderModel.AssignValuesToBlocks(_replaceBooksViewModel);
            
            _labelModel = new LabelsModel();

            _arrowModel = new ArrowModel();
            
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Image arrow in _arrowModel._directionArrowsList)
            {
                ReplacingBooks.Children.Add(arrow);
            }
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Border border in _borderModel._CallBlockBordersList)
            {
                ReplacingBooks.Children.Add(border);
            }
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Label label in _labelModel._currentStorageLevelList)
            {
                ReplacingBooks.Children.Add(label);
            }
            _onSettingsPage = false;
        }
        
        //To colour block strokes
        private void ActivateBlockColour(Rectangle rect, int mode)
        {
            //If left stack size is 0
            if (_replaceBooksViewModel.CallNumberStacks.ElementAt(2).Count < 1)
            {
                _arrowModel.DisableArrows(2);
            }            
            //if right stack size is 0
            if (_replaceBooksViewModel.CallNumberStacks.ElementAt(3).Count < 1)
            {
                _arrowModel.DisableArrows(3);
            }
            
            switch (mode)
            {
                case 0: 
                    _replaceBooksViewModel.BlackBrush = new SolidColorBrush(Colors.Gold); //Sender
                    _replaceBooksViewModel.GameCounts[1]++; //Program now knows start, waiting for destination 
                    break;
                case 1:
                    _replaceBooksViewModel.BlackBrush = new SolidColorBrush(Colors.Blue); //Reciever
                    break;
                case 2:
                    _replaceBooksViewModel.BlackBrush = new SolidColorBrush(Colors.Red); //Reciever
                    break;
                default:
                    _replaceBooksViewModel.BlackBrush = new SolidColorBrush(Colors.Transparent); //Error
                    break;
            }
            rect.StrokeThickness = 3;
            rect.Stroke = _replaceBooksViewModel.BlackBrush;
        }

        private void ReplacingBooks_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        //To make a block show red when an error occurs
        private void ActivateRedError(Rectangle rect, string errorText)
        {
            ActivateBlockColour(rect, 2); //Display Red
            MessageBox.Show("Action not allowed " + errorText);
            ClearAllFocus();
            _replaceBooksViewModel.GameCounts[1] = 0;//Active Block Count
        }

        //To clear all colours surrounding blocks
        private void ClearAllFocus()
        {
            ActivateBlockColour(selectTopRect, 3); //Make Transparent
            ActivateBlockColour(selectBottomRect, 3); //Make Transparent
            ActivateBlockColour(selectLeftRect, 3); //Make Transparent
            ActivateBlockColour(selectRightRect, 3); //Make Transparent
            _replaceBooksViewModel.GameCounts[1] = 0;//Active Block Count
            _replaceBooksViewModel.RectangleNumber[1] = 0;//Destination Rectangle Number
        }

        //To push a block number from one stack to another
        private void UpdateStack(int rectangleNumber)
        {
            AnimateBlockMovement();
            _replaceBooksViewModel.GameCounts[0]++;
            txtMovesCount.Content = "Moves: " + _replaceBooksViewModel.GameCounts[0];
            _replaceBooksViewModel.PushCallNumber(rectangleNumber,_replaceBooksViewModel.RectangleNumber[0]);
            ClearAllFocus();

            _arrowModel.UpdateArrowsEasyMode(_replaceBooksViewModel);
            _labelModel.UpdateCapacityLabels(_replaceBooksViewModel);
            
        }

        private void SelectedRectangle(Rectangle currentRectangle, Stack<double> currentRectangleStack,
            int currentRectangleNumber)
        {
            var isEmptyRect = !currentRectangleStack.Any(); //check if the list is empty
            if (_replaceBooksViewModel.GameCounts[1] == 0) //This is start block
            {
                //check num of blocks within rect
                if (isEmptyRect) //No more blocks to move
                {
                    ActivateRedError(currentRectangle, "This rectangle is empty, cannot send anything from it");
                }
                else
                {
                    _replaceBooksViewModel.RectangleNumber[0] = currentRectangleNumber; //Sets origin rectangle stack
                    ActivateBlockColour(currentRectangle, 0); //Display Gold as this is start block
                }
            }
            else //this is destination block
            {
                //check num of blocks within rect
                if (isEmptyRect) //can add block
                {
                    if (_replaceBooksViewModel.RectangleNumber[0] != currentRectangleNumber) //Make sure the source and the destination are not the same
                    {
                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                        _replaceBooksViewModel.RectangleNumber[1] = currentRectangleNumber; //Set destination stack 
                        UpdateStack(currentRectangleNumber);
                    }
                    else
                    {
                        ActivateRedError(currentRectangle, "You cannot send a call number to the same start");
                    }
                }
                else
                {
                    if (_replaceBooksViewModel.RectangleNumber[0] == currentRectangleNumber)//If both source and destination are the same
                    {
                        ActivateRedError(currentRectangle, "You cannot send a call number to the same start");
                    }
                    else
                    { 
                        if (currentRectangleStack.Count < _replaceBooksViewModel.StackSizes[currentRectangleNumber]) //If there is still space for the block
                        {
                            if (currentRectangleStack.Count == 1 && _replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(currentRectangleNumber*2) &&_replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(currentRectangleNumber*2+1)) //If there is only one block inside and no rules on block
                            {
                                ActivateBlockColour(currentRectangle, 1); //Make Blue
                                _replaceBooksViewModel.RectangleNumber[1] = currentRectangleNumber; //Set destination 
                                UpdateStack(currentRectangleNumber);
                            }
                            else
                            {
                                //What is my order
                                if (_replaceBooksViewModel.RectangleSortOrder[currentRectangleNumber] == 'A' || _replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(currentRectangleNumber*2)) //If ascending
                                {
                                    if (_replaceBooksViewModel.CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[1]).Peek()>
                                        _replaceBooksViewModel.CallNumberStacks.ElementAt(currentRectangleNumber).Peek()) //If new addition is greater than top num
                                    {
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _replaceBooksViewModel.RectangleNumber[1] = currentRectangleNumber; //Set destination 
                                        UpdateStack(currentRectangleNumber);
                                    }
                                    else
                                    {
                                        ActivateRedError(currentRectangle, "This is a ascending list");
                                    }
                                }
                                else if (_replaceBooksViewModel.RectangleSortOrder[currentRectangleNumber] == 'D'||  _replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(currentRectangleNumber*2+1)) //If Descending
                                {
                                    if (_replaceBooksViewModel.CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[1]).Peek()<
                                        _replaceBooksViewModel.CallNumberStacks.ElementAt(currentRectangleNumber).Peek()) //If new addition is greater than top num
                                    {
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _replaceBooksViewModel.RectangleNumber[1] = currentRectangleNumber; //Set destination 
                                        UpdateStack(currentRectangleNumber);
                                    }
                                    else
                                    {
                                        ActivateRedError(currentRectangle, "This is a descending list");
                                    }
                                }

                                //Does it match my order?
                            }
                        }
                        else
                        {
                            ActivateRedError(currentRectangle, "Full");
                        }   
                    }

                }
            }
        }

        private void selectTopRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectTopRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(0), 0);
        }

        private void selectBottomRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectBottomRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(1), 1);
        }

        private void selectLeftRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectLeftRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(2), 2);
        }

        private void selectRightRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(selectRightRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(3), 3);
        }

        //For both left and right rectangles
        private int ReturnCurrentBlockYLeftRight(Stack<double> rectStack)
        {
            switch (rectStack.Count + 1)
            {
                case 1: //Rectangle at bottom of rect list
                    return 320;
                case 2: //Second rectangle
                    return 293;
                case 3:
                    return 266;
                case 4:
                    return 239;
                case 5:
                    return 212;
                default: //Last rectangle
                    return 185;
            }
        }

        private void MoveRectangle(int blockNumber, int originRectangle)
        {
            switch (originRectangle)
            {
                case 0:
                    originRectangle = 60;
                    break;
                case 1:
                    originRectangle = 285;
                    break;
                case 2:
                case 3:
                    originRectangle = 180;
                    break;
            }
            StartYJourney(_borderModel._CallBlockBordersList.ElementAt(blockNumber-1), originRectangle);
        }

        private void StartXJourneyRight(Border border, int stop)
        {
            do
            {
                Canvas.SetLeft(border, Canvas.GetLeft(border) + 1);
            } while (Canvas.GetLeft(border) < stop);
        }

        private void StartXJourneyLeft(Border border, int stop)
        {
            do
            {
                Canvas.SetLeft(border, Canvas.GetLeft(border) - 1);
            } while (Canvas.GetLeft(border) > stop);
        }

        private void StartYJourneyUp(Border border)
        {
            var destinationY = 0;
            switch (_replaceBooksViewModel.RectangleNumber[1])
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(0), _replaceBooksViewModel.TopRectCanvasYLocations);
                    break;
                case 1: // For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(1), _replaceBooksViewModel.BottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                case 3:
                    destinationY = ReturnCurrentBlockYLeftRight(_replaceBooksViewModel.CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[1]));
                    break;
            }
            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) + 1);
            } while (Canvas.GetTop(border) < destinationY); //285<320
            if (Canvas.GetTop(border) > destinationY) //320 <320  285< 218
                do
                {
                    Canvas.SetTop(border, Canvas.GetTop(border) - 1);
                } while (Canvas.GetTop(border) > destinationY); //285>320  285>218
            //Check destination amount of elements
        }

        private void StartYJourneyDown(Border border)
        {
            var destinationY = 0;
            switch (_replaceBooksViewModel.RectangleNumber[1])
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(0), _replaceBooksViewModel.TopRectCanvasYLocations);
                    break;
                case 1: // For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(1), _replaceBooksViewModel.BottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                case 3:
                    destinationY = ReturnCurrentBlockYLeftRight(_replaceBooksViewModel.CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[1]));
                    break;
            }
            
            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) + 1);
            } while (Canvas.GetTop(border) < destinationY);
            //Check destination amount of elements
        }

        //https://stackoverflow.com/questions/10298216/moving-any-control-in-wpf
        private void StartYJourney(Border border, int top)
        {
            //Move up based on current location to top of current rectangle
            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) - 1);
            } while (Canvas.GetTop(border) > top);

            //Determine if need to be going left or right
            
            switch (_replaceBooksViewModel.RectangleNumber[1]) //0 Top, 1 Bottom, 2 Left, 3 Right Destination
            {
                case 0: //Going to top
                    switch (_replaceBooksViewModel.RectangleNumber[0])//Origin
                    {
                        case 1: //Starting from bottom
                            StartYJourneyUp(border); //Just go straight up
                            break;
                        case 2: //Starting from left
                            
                            StartXJourneyRight(border, 321);
                            //Then go right until reach 321
                            StartYJourneyUp(border); //Just go straight up
                            break;
                        case 3: //Starting from right
                            StartXJourneyLeft(border, 321);
                            //Then go left until reach 321
                            StartYJourneyUp(border); //Just go straight up
                            break;
                    }
                    break;
                case 1: //Going to Bottom 285
                    switch (_replaceBooksViewModel.RectangleNumber[0])
                    {
                        case 0: //Starting from Top
                            //Just go straight down
                            StartYJourneyDown(border);
                            break;
                        case 2: //Starting from left
                            //Go right until reach 321
                            StartXJourneyRight(border, 321);
                            //Then go down until reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 3: //Starting from right
                            //Go left until reach 321
                            StartXJourneyLeft(border, 321);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }

                    break;
                case 2: //Going to Left 180
                    switch (_replaceBooksViewModel.RectangleNumber[0])
                    {
                        case 0: //Starting from top
                            //Go left until 175
                            StartXJourneyLeft(border, 175);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 1: //Starting from bottom
                            StartXJourneyLeft(border, 175);
                            //Then go down reach saved location
                            StartYJourneyUp(border);
                            break;
                        case 3: //Starting from right
                            StartXJourneyLeft(border, 175);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }

                    break;
                case 3: //Going to Right one
                    switch (_replaceBooksViewModel.RectangleNumber[0])
                    {
                        //478
                        case 0: //Starting from top
                            //Go right until 478
                            StartXJourneyRight(border, 478);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                        case 1: //Starting from bottom
                            //Go up until top of right and left
                            //Go right until 478
                            StartXJourneyRight(border, 478);
                            //Then go down reach saved location
                            StartYJourneyUp(border);
                            break;
                        case 2: //Starting from left
                            //Go right until 478
                            StartXJourneyRight(border, 478);
                            //Then go down reach saved location
                            StartYJourneyDown(border);
                            break;
                    }

                    break;
            }
        }

        private int ReturnCurrentBlockYTopBottom(Stack<double> rectStack, int[] locations)
        {
            int location; //To store where to place the block
            if (rectStack.Any())//If there are elements within the stack
            {
                location = rectStack.Count;
            }
            else
                location = 0;
            return locations[location];
        }

        private void AnimateBlockMovement()
        {
            //Based on original rectangle number between 0-3
            for (var i = 0; i < _replaceBooksViewModel.RectValueNamePair.Count; i++)
                if (_replaceBooksViewModel.RectValueNamePair.Keys.ElementAt(i) == _replaceBooksViewModel.CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[0]).Peek())
                {
                    MoveRectangle(_replaceBooksViewModel.RectValueNamePair.Values.ElementAt(i), _replaceBooksViewModel.RectangleNumber[0]);
                    break;
                }
            
        }

        private void BtnReset_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _replaceBooksViewModel = new ReplaceBooksViewModel();
        }
        private void BtnSettings_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Change whatever you want
            txtMovesCount.Visibility = Visibility.Collapsed;
            btnReset.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Collapsed;

            btnSaveSettings.Visibility = Visibility.Visible;
            btnCloseSettings.Visibility = Visibility.Visible;
            //btnSettings.Source = new BitmapImage(new Uri(@"/Theme/Assets/Save.png", UriKind.Relative));;

            for (int i = 0; i < 4; i++)
            {
                Canvas.SetLeft(_labelModel._currentStorageLevelList.ElementAt(i),
                    Canvas.GetLeft(_labelModel._currentStorageLevelList.ElementAt(i))-10);
                _labelModel._currentStorageLevelList.ElementAt(i).Content = "Size:";
                
            }
            

            selectTopRect.Visibility = Visibility.Collapsed;
            selectBottomRect.Visibility = Visibility.Collapsed;
            selectLeftRect.Visibility = Visibility.Collapsed;
            selectRightRect.Visibility = Visibility.Collapsed;

            for (int i = 0; i < 10; i++)
            {
                _borderModel._CallBlockBordersList.ElementAt(i).Visibility = Visibility.Collapsed;
            }

            _onSettingsPage = true;
            UpdateDifficultyUiElements();
        }
        private void UpdateDifficultyUiElements()
        {
            if (_replaceBooksViewModel.CurrentDifficulty > 5)
            {
                _replaceBooksViewModel.CurrentDifficulty = 0;
              
            }
            _replaceBooksViewModel.UpdateDifficulty();
            switch (_replaceBooksViewModel.CurrentDifficulty)
            {
                case 1:
                    UpdateStackSizeText(5, 5, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Normal.png", UriKind.Relative));
                    break;
                case 2:
                    UpdateStackSizeText(6, 6, 6, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Difficult.png", UriKind.Relative));
                    break;
                case 3:
                    UpdateStackSizeText(6, 6, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Hard.png", UriKind.Relative));
                    break;
                case 4:
                    UpdateStackSizeText(6, 5, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Extreme.png", UriKind.Relative));
                    break;
                case 5:
                    UpdateStackSizeText(5, 5, 5, 5);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Insane.png", UriKind.Relative));
                    break;
                default:
                    UpdateStackSizeText(6, 6, 6, 6);
                    imgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Easy.png", UriKind.Relative));
                    break;;
            }

            _arrowModel.UpdateArrows(_replaceBooksViewModel);
            
        }
        private void ImgDifficulty_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_replaceBooksViewModel.OnSettingsPage)
            {
                _replaceBooksViewModel.CurrentDifficulty++;
                UpdateDifficultyUiElements();
            }
        }
       
        private void UpdateStackSizeText(int top,int bot,int left,int right)
        {

            _replaceBooksViewModel.StackSizes[0] = top;
            _replaceBooksViewModel.StackSizes[1] = bot;
            _replaceBooksViewModel.StackSizes[2] = left;
            _replaceBooksViewModel.StackSizes[3] = right;
        }
        private void BtnSaveSettings_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void BtnCloseSettings_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtMovesCount.Visibility = Visibility.Visible;
            btnReset.Visibility = Visibility.Visible;
            btnSettings.Visibility = Visibility.Visible;

            btnSaveSettings.Visibility = Visibility.Collapsed;
            btnCloseSettings.Visibility = Visibility.Collapsed;
            
            for (int i = 0; i < 4; i++)
            {
                Canvas.SetLeft(_labelModel._currentStorageLevelList.ElementAt(i),
                    Canvas.GetLeft(_labelModel._currentStorageLevelList.ElementAt(i))+10);
                _labelModel._currentStorageLevelList.ElementAt(i).Content = 
                (_replaceBooksViewModel.CallNumberStacks.ElementAt(i).Count / _replaceBooksViewModel.StackSizes[i])*100 + "%";
            }

            selectTopRect.Visibility = Visibility.Visible;
            selectBottomRect.Visibility = Visibility.Visible;
            selectLeftRect.Visibility = Visibility.Visible;
            selectRightRect.Visibility = Visibility.Visible;

            for (int i = 0; i < 10; i++)
            {
                _borderModel._CallBlockBordersList.ElementAt(i).Visibility = Visibility.Visible;
            }

            _onSettingsPage = false;
        }

    }
}