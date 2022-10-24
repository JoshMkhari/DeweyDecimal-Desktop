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
        int[] leftValues = new []{ 32,132,232,326,424,522,614};
        int[] topValues = new []{ 90,194,90,194,90,194,90 };


        public RimNetModel()
        {
            NetLocationList = new List<Image>();
            CreateRims();
        }

        private void CreateRims()
        {
            //Creates Rims
            for (int i = 0; i < 7; i++)
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
                Canvas.SetTop(currentImage,topValues[i]);
                Panel.SetZIndex(currentImage,10);
                NetLocationList.Add(currentImage);
            }

        }
    }
}