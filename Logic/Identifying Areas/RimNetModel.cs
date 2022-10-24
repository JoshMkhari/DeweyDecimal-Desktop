using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace JoshMkhariPROG7312Game.Logic.Identifying_Areas
{
    public class RimNetModel
    {
        public List<Image> NetLocationList;
        private int[] leftValues = new []{ 32,388, 526 };


        public RimNetModel()
        {
            NetLocationList = new List<Image>();
            CreateRims();
        }

        private void CreateRims()
        {
            //Creates Rims
            for (int i = 0; i < 1; i++)
            {
                Image currentImage = new Image
                {
                    Width = 80,
                    Height = 80,
                    Name = "Ball"+i,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Source = new BitmapImage(new Uri(@"/Theme/Assets/net.png", UriKind.Relative))
                };
                Canvas.SetLeft(currentImage,leftValues[i]); 
                Canvas.SetTop(currentImage,90);
                Panel.SetZIndex(currentImage,10);
                NetLocationList.Add(currentImage);
            }

        }
    }
}