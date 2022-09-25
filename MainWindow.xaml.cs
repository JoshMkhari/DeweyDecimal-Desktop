using System.Windows;
using System.Windows.Input;

namespace JoshMkhariPROG7312Game
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ToggleButtonReplacingBooks_OnChecked(object sender, RoutedEventArgs e)
        {
            TopCoolLines.Visibility = Visibility.Collapsed;
        }

        private void ToggleButtonHome_OnChecked(object sender, RoutedEventArgs e)
        {
            TopCoolLines.Visibility = Visibility.Visible;
        }
    }
}