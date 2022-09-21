using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JoshMkhariPROG7312Game.Logic.Home;

namespace JoshMkhariPROG7312Game.Views
{
    /// <summary>
    ///     Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView
    {
        private TextBlockModel _textBlockModel;
        public HomeView()
        {
            InitializeComponent();
            _textBlockModel = new TextBlockModel();
            foreach (TextBlock textBlock in TextBlockModel.TextBlocksList)
            {
                WinHistoryStackPanel.Children.Add(textBlock);
            }
        }

        private void ReplacingBooksTutorial_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Hide all other elements
            StackPanelBackground.Visibility = Visibility.Collapsed;
            PageNameTitleTextBlock.Text = "Replacing books tutorial";
            PageNameTitleTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            CanvasBack.Background =
                new SolidColorBrush(Color.FromRgb(246,239,231)); //https://www.rapidtables.com/convert/color/hex-to-rgb.html
            ReplacingBooksBackground.Visibility = Visibility.Visible;
            BtnCloseTutorial.Visibility = Visibility.Visible;

        }

        private void BtnCloseTutorial_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Hide all tutorial elements
            StackPanelBackground.Visibility = Visibility.Visible;
            PageNameTitleTextBlock.Text = "Home";
            PageNameTitleTextBlock.Foreground = new SolidColorBrush(Colors.White);
            CanvasBack.Background =
                new SolidColorBrush(Colors.Transparent); //https://www.rapidtables.com/convert/color/hex-to-rgb.html
            ReplacingBooksBackground.Visibility = Visibility.Collapsed;
            BtnCloseTutorial.Visibility = Visibility.Collapsed;
        }
    }
}