using System.Windows.Controls;
using JoshMkhariPROG7312Game.Logic.FindCallNumbers;

namespace JoshMkhariPROG7312Game.Views
{
    public partial class FindCallNumbers : UserControl
    {
        public FindCallNumbers()
        {
            InitializeComponent();
            DeweySystem deweySystem = new DeweySystem();
        }
    }
}