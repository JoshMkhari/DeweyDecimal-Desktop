using System.Linq;
using System.Windows.Controls;
using JoshMkhariPROG7312Game.Logic.Identifying_Areas;

namespace JoshMkhariPROG7312Game.Views
{
    public partial class IdentifyingAreas : UserControl
    {
        public IdentifyingAreas()
        {
            InitializeComponent();
            //https://stackoverflow.com/questions/11485843/how-can-i-create-hexagon-menu-using-wpf

            HexagonModel hexagonModel = new HexagonModel();

            IdentifyAreaCanvas.Children.Add(hexagonModel.HexagonList.ElementAt(0));
        }
    }
}