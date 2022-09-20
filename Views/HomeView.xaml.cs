using System.Diagnostics;
using System.Windows.Controls;
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
    }
}