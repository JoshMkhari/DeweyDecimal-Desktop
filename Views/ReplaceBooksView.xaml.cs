using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ReplaceBooksView
    {
        //Declarations
        private ReplaceBooksViewModel _replaceBooksViewModel; //Used to access all variables and methods within model

        private readonly BorderModel _borderModel;
        private readonly LabelsModel _labelModel;

        private readonly ArrowModel _arrowModel;
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
            foreach (Image arrow in _arrowModel.DirectionArrowsList)
            {
                ReplacingBooks.Children.Add(arrow);
            }

            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Border border in _borderModel.CallBlockBordersList)
            {
                ReplacingBooks.Children.Add(border);
            }

            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Label label in _labelModel.CurrentStorageLevelList)
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
                    _replaceBooksViewModel.BlackBrush = new SolidColorBrush(Colors.Blue); //Receiver
                    break;
                case 2:
                    _replaceBooksViewModel.BlackBrush = new SolidColorBrush(Colors.Red); //Receiver
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
            _replaceBooksViewModel.GameCounts[1] = 0; //Active Block Count
        }

        //To clear all colours surrounding blocks
        private void ClearAllFocus()
        {
            ActivateBlockColour(SelectTopRect, 3); //Make Transparent
            ActivateBlockColour(SelectBottomRect, 3); //Make Transparent
            ActivateBlockColour(SelectLeftRect, 3); //Make Transparent
            ActivateBlockColour(SelectRightRect, 3); //Make Transparent
            _replaceBooksViewModel.GameCounts[1] = 0; //Active Block Count
            _replaceBooksViewModel.RectangleNumber[1] = 0; //Destination Rectangle Number
        }

        //To push a block number from one stack to another
        private void UpdateStack(int rectangleNumber)
        {
            AnimateBlockMovement();
            _replaceBooksViewModel.GameCounts[0]++;
            TxtMovesCount.Content = "Moves: " + _replaceBooksViewModel.GameCounts[0];
            _replaceBooksViewModel.PushCallNumber(rectangleNumber, _replaceBooksViewModel.RectangleNumber[0]);
            ClearAllFocus();

            _arrowModel.UpdateArrowsEasyMode(_replaceBooksViewModel);
            _labelModel.UpdateCapacityLabels(_replaceBooksViewModel);

            if (_replaceBooksViewModel.CallNumberStacks.ElementAt(0).Count == 5 && //Top Stack must have 5 elements
                _replaceBooksViewModel.CallNumberStacks.ElementAt(1).Count == 5 && //Bottom Stack must have 5 elements
                _replaceBooksViewModel.RectangleSortOrder[0] == 'A' && //Top Stack must be storing in Ascending order
                _replaceBooksViewModel.RectangleSortOrder[1] == 'A') //Bottom Stack must be storing in Ascending order
            {
                bool finalCheck = true;
                for (int i = 0; i < 2; i++)
                {
                    if (CheckIfAscending(_replaceBooksViewModel.CallNumberStacks.ElementAt(i))) continue;
                    finalCheck = false;
                    break;
                }

                if (finalCheck)
                {
                    MessageBox.Show(CheckIfWon(0) == 5
                        ? "You win"
                        : "The call blocks are not correctly sorted, try again");
                }
                else
                {
                    MessageBox.Show("The call blocks are not correctly sorted, try again");
                }
            }
        }

        //Recursive method to determine if game is won
        private int CheckIfWon(int current)
        {
            if (current == 5) return current;
            return _replaceBooksViewModel.CallNumberStacks.ElementAt(0).ElementAt(current) >
                   _replaceBooksViewModel.CallNumberStacks.ElementAt(1).ElementAt(current)
                ? CheckIfWon(current + 1)
                : current;
        }

        private bool CheckIfAscending(Stack<double> currentStack)
        {
            int index = 0;
            while (index < 5)
            {
                if (currentStack.ElementAt(index) < currentStack.ElementAt(index + 1))
                {
                    index++;
                }
                else
                    return false;
            }

            return true;
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
                    if (_replaceBooksViewModel.RectangleNumber[0] !=
                        currentRectangleNumber) //Make sure the source and the destination are not the same
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
                    if (_replaceBooksViewModel.RectangleNumber[0] ==
                        currentRectangleNumber) //If both source and destination are the same
                    {
                        ActivateRedError(currentRectangle, "You cannot send a call number to the same start");
                    }
                    else
                    {
                        if (currentRectangleStack.Count <
                            _replaceBooksViewModel.StackSizes
                                [currentRectangleNumber]) //If there is still space for the block
                        {
                            if (currentRectangleStack.Count == 1 &&
                                _replaceBooksViewModel.ActiveAscDescStacks.Values
                                    .ElementAt(currentRectangleNumber * 2) &&
                                _replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(currentRectangleNumber * 2 +
                                    1)) //If there is only one block inside and no rules on block
                            {
                                ActivateBlockColour(currentRectangle, 1); //Make Blue
                                _replaceBooksViewModel.RectangleNumber[1] = currentRectangleNumber; //Set destination 
                                UpdateStack(currentRectangleNumber);
                            }
                            else
                            {
                                //What is my order
                                if (_replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(currentRectangleNumber)
                                    && !_replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(
                                        currentRectangleNumber + 4))
                                {
                                    _replaceBooksViewModel.RectangleSortOrder[currentRectangleNumber] = 'A';
                                }

                                if (_replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(currentRectangleNumber +
                                        4)
                                    && !_replaceBooksViewModel.ActiveAscDescStacks.Values.ElementAt(
                                        currentRectangleNumber))
                                {
                                    _replaceBooksViewModel.RectangleSortOrder[currentRectangleNumber] = 'D';
                                }

                                switch (_replaceBooksViewModel.RectangleSortOrder[currentRectangleNumber])
                                {
                                    //If ascending
                                    //If new addition is greater than top num
                                    case 'A' when _replaceBooksViewModel.CallNumberStacks
                                                      .ElementAt(_replaceBooksViewModel.RectangleNumber[0]).Peek() >
                                                  _replaceBooksViewModel.CallNumberStacks
                                                      .ElementAt(currentRectangleNumber).Peek():
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _replaceBooksViewModel.RectangleNumber[1] =
                                            currentRectangleNumber; //Set destination 
                                        UpdateStack(currentRectangleNumber);
                                        break;
                                    case 'A':
                                        ActivateRedError(currentRectangle, "This is a ascending list");
                                        break;
                                    //If Descending
                                    //If new addition is greater than top num
                                    case 'D' when _replaceBooksViewModel.CallNumberStacks
                                                      .ElementAt(_replaceBooksViewModel.RectangleNumber[0]).Peek() <
                                                  _replaceBooksViewModel.CallNumberStacks
                                                      .ElementAt(currentRectangleNumber).Peek():
                                        ActivateBlockColour(currentRectangle, 1); //Make Blue
                                        _replaceBooksViewModel.RectangleNumber[1] =
                                            currentRectangleNumber; //Set destination 
                                        UpdateStack(currentRectangleNumber);
                                        break;
                                    case 'D':
                                        ActivateRedError(currentRectangle, "This is a descending list");
                                        break;
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
            SelectedRectangle(SelectTopRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(0), 0);
        }

        private void selectBottomRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(SelectBottomRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(1), 1);
        }

        private void selectLeftRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(SelectLeftRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(2), 2);
        }

        private void selectRightRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedRectangle(SelectRightRect, _replaceBooksViewModel.CallNumberStacks.ElementAt(3), 3);
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

            StartYJourney(_borderModel.CallBlockBordersList.ElementAt(blockNumber - 1), originRectangle);
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
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(0),
                        _replaceBooksViewModel.TopRectCanvasYLocations);
                    break;
                case 1: // For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(1),
                        _replaceBooksViewModel.BottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                case 3:
                    destinationY = ReturnCurrentBlockYLeftRight(
                        _replaceBooksViewModel.CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[1]));
                    break;
            }

            do
            {
                Canvas.SetTop(border, Canvas.GetTop(border) + 1);
            } while (Canvas.GetTop(border) < destinationY); //285<320

            while (Canvas.GetTop(border) > destinationY)
            {
                Canvas.SetTop(border, Canvas.GetTop(border) - 1);
            }
            //Check destination amount of elements
        }

        private void StartYJourneyDown(Border border)
        {
            var destinationY = 0;
            switch (_replaceBooksViewModel.RectangleNumber[1])
            {
                case 0: //For Top Block
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(0),
                        _replaceBooksViewModel.TopRectCanvasYLocations);
                    break;
                case 1: // For Bottom block
                    destinationY = ReturnCurrentBlockYTopBottom(_replaceBooksViewModel.CallNumberStacks.ElementAt(1),
                        _replaceBooksViewModel.BottomRectCanvasYLocations);
                    break;
                case 2: //For left Block
                case 3:
                    destinationY = ReturnCurrentBlockYLeftRight(
                        _replaceBooksViewModel.CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[1]));
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
                    switch (_replaceBooksViewModel.RectangleNumber[0]) //Origin
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
            //To store where to place the block
            int location = rectStack.Any() ? rectStack.Count : 0; //If there are elements within the stack
            return locations[location];
        }

        private void AnimateBlockMovement()
        {
            //Based on original rectangle number between 0-3
            for (var i = 0; i < _replaceBooksViewModel.RectValueNamePair.Count; i++)
                if (_replaceBooksViewModel.RectValueNamePair.Keys.ElementAt(i) == _replaceBooksViewModel
                        .CallNumberStacks.ElementAt(_replaceBooksViewModel.RectangleNumber[0]).Peek())
                {
                    MoveRectangle(_replaceBooksViewModel.RectValueNamePair.Values.ElementAt(i),
                        _replaceBooksViewModel.RectangleNumber[0]);
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
            TxtMovesCount.Visibility = Visibility.Collapsed;
            BtnReset.Visibility = Visibility.Collapsed;
            BtnSettings.Visibility = Visibility.Collapsed;

            BtnSaveSettings.Visibility = Visibility.Visible;
            BtnCloseSettings.Visibility = Visibility.Visible;

            for (int i = 0; i < 4; i++)
            {
                Canvas.SetLeft(_labelModel.CurrentStorageLevelList.ElementAt(i),
                    Canvas.GetLeft(_labelModel.CurrentStorageLevelList.ElementAt(i)) - 10);
                _labelModel.CurrentStorageLevelList.ElementAt(i).Content = "Size:";
            }


            SelectTopRect.Visibility = Visibility.Collapsed;
            SelectBottomRect.Visibility = Visibility.Collapsed;
            SelectLeftRect.Visibility = Visibility.Collapsed;
            SelectRightRect.Visibility = Visibility.Collapsed;

            for (int i = 0; i < 10; i++)
            {
                _borderModel.CallBlockBordersList.ElementAt(i).Visibility = Visibility.Collapsed;
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
                    ImgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Normal.png", UriKind.Relative));
                    break;
                case 2:
                    UpdateStackSizeText(6, 6, 6, 5);
                    ImgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Difficult.png", UriKind.Relative));
                    break;
                case 3:
                    UpdateStackSizeText(6, 6, 5, 5);
                    ImgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Hard.png", UriKind.Relative));
                    break;
                case 4:
                    UpdateStackSizeText(6, 5, 5, 5);
                    ImgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Extreme.png", UriKind.Relative));
                    break;
                case 5:
                    UpdateStackSizeText(5, 5, 5, 5);
                    ImgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Insane.png", UriKind.Relative));
                    break;
                default:
                    UpdateStackSizeText(6, 6, 6, 6);
                    ImgDifficulty.Source = new BitmapImage(new Uri(@"/Theme/Assets/Easy.png", UriKind.Relative));
                    break;
            }

            _arrowModel.UpdateArrows(_replaceBooksViewModel);
        }

        private void ImgDifficulty_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_onSettingsPage)
            {
                _replaceBooksViewModel.CurrentDifficulty++;
                UpdateDifficultyUiElements();
            }
        }

        private void UpdateStackSizeText(int top, int bot, int left, int right)
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
            TxtMovesCount.Visibility = Visibility.Visible;
            BtnReset.Visibility = Visibility.Visible;
            BtnSettings.Visibility = Visibility.Visible;

            BtnSaveSettings.Visibility = Visibility.Collapsed;
            BtnCloseSettings.Visibility = Visibility.Collapsed;

            for (int i = 0; i < 4; i++)
            {
                Canvas.SetLeft(_labelModel.CurrentStorageLevelList.ElementAt(i),
                    Canvas.GetLeft(_labelModel.CurrentStorageLevelList.ElementAt(i)) + 10);
                _labelModel.CurrentStorageLevelList.ElementAt(i).Content =
                    Math.Round(_replaceBooksViewModel.CallNumberStacks.ElementAt(i).Count /
                        _replaceBooksViewModel.StackSizes[i] * 100) + "%";
            }

            SelectTopRect.Visibility = Visibility.Visible;
            SelectBottomRect.Visibility = Visibility.Visible;
            SelectLeftRect.Visibility = Visibility.Visible;
            SelectRightRect.Visibility = Visibility.Visible;

            for (int i = 0; i < 10; i++)
            {
                _borderModel.CallBlockBordersList.ElementAt(i).Visibility = Visibility.Visible;
            }

            _onSettingsPage = false;
        }
    }
}