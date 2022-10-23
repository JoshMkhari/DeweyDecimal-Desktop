using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
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
            
            //https://stackoverflow.com/questions/51594536/add-a-textbox-to-a-wpf-canvas-programmatically
            foreach (Path hex in hexagonModel.HexagonList)
            {
                hex.MouseLeftButtonDown += OnHexClick;//https://stackoverflow.com/questions/22359525/creating-mouseleftbuttondown-for-dynamically-created-rectangles-in-wpf
                IdentifyAreaCanvas.Children.Add(hex);
                
            }
            
        }

        private void OnHexClick(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/67609123/wpf-c-sharp-create-click-event-for-dynamically-created-button
            Path currentHex = (Path)sender;
            string name = currentHex.Name;
            Debug.WriteLine(name);
            switch (name)
            {
                case "Hex0":
                {
                    Debug.WriteLine("Mama i made it");
                    break;
                }
            }
        }
    }
}