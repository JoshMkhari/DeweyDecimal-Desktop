using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using JoshMkhariPROG7312Game.ViewModels;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class LabelsModel
    {
        public LabelsModel()
        {
            int[] leftValues = { 332, 184 };
            int[] topValues = { 340, 218 };

            LabelDefaults = new int[2][];
            LabelDefaults[0] = leftValues;
            LabelDefaults[1] = topValues;

            CurrentStorageLevelList = new List<Label>();
            CreateStorageCapacityLabels();
        }

        private int[][] LabelDefaults { get; } //https://www.geeksforgeeks.org/c-sharp-jagged-arrays/
        public List<Label> CurrentStorageLevelList { get; }

        public void UpdateCapacityLabels(ReplaceBooksViewModel replaceBooksViewModel)
        {
            for (var i = 0; i < 4; i++)
                CurrentStorageLevelList.ElementAt(i).Content =
                    Math.Round(replaceBooksViewModel.CallNumberStacks.ElementAt(i).Count /
                        replaceBooksViewModel.StackSizes[0] * 100) + "%";
        }

        private void CreateStorageCapacityLabels()
        {
            //Creates top and bottom labels
            for (var i = 0; i < 2; i++)
            {
                var currentLabel = new Label
                {
                    FontSize = 20
                };
                Canvas.SetLeft(currentLabel, LabelDefaults[0][0]);
                Canvas.SetTop(currentLabel, LabelDefaults[1][1] + i * 226);
                CurrentStorageLevelList.Add(currentLabel);
            }

            //creates left and right labels
            for (var i = 0; i < 2; i++)
            {
                var currentLabel = new Label
                {
                    FontSize = 20
                };
                Canvas.SetLeft(currentLabel, LabelDefaults[0][1] + i * 304);
                Canvas.SetTop(currentLabel, LabelDefaults[1][0]);
                CurrentStorageLevelList.Add(currentLabel);
            }
        }
    }
}