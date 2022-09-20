using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using JoshMkhariPROG7312Game.ViewModels;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class LabelsModel
    {
        private int[][] LabelDefaults { get; }//https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        public List<Label> CurrentStorageLevelList { get; }
        public LabelsModel()
        {
            int[] leftValues = new []{ 332, 184 };
            int[] topValues = new []{ 340, 218 };
            
            LabelDefaults = new int[2][];
            LabelDefaults[0] = leftValues;
            LabelDefaults[1] = topValues;
            
            CurrentStorageLevelList = new List<Label>();
            CreateStorageCapacityLabels();
        }

        public void UpdateCapacityLabels(ReplaceBooksViewModel replaceBooksViewModel)
        {
            for (int i = 0; i < 4; i++)
            {
                CurrentStorageLevelList.ElementAt(i).Content = (replaceBooksViewModel.CallNumberStacks.ElementAt(i).Count / replaceBooksViewModel.StackSizes[0])*100 + "%";
            }
        }
        private void CreateStorageCapacityLabels()
        {
            //Creates top and bottom labels
            for (int i = 0; i < 2; i++)
            {
                Label currentLabel = new Label();
                Canvas.SetLeft(currentLabel,LabelDefaults[0][0]);
                Canvas.SetTop(currentLabel,LabelDefaults[1][1]+(i*226));
                CurrentStorageLevelList.Add(currentLabel);
            }
            
            //creates left and right labels
            for (int i = 0; i < 2; i++)
            {
                Label currentLabel = new Label();
                Canvas.SetLeft(currentLabel,LabelDefaults[0][1]+(i*304));
                Canvas.SetTop(currentLabel,LabelDefaults[1][0]);
                CurrentStorageLevelList.Add(currentLabel);
            }


        }
    }
}