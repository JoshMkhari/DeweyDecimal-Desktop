using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace JoshMkhariPROG7312Game.Logic.Identifying_Areas
{
    public class BasketBallModel
    {
        public List<Image> BallLocationList;
        private int[] leftValues = new []{ 256,388, 526 };

        public BasketBallModel()
        {
            BallLocationList = new List<Image>();
            CreateBasketBalls();
        }

        private void CreateBasketBalls()
        {
            //Creates BasketBalls
            for (int i = 0; i < 3; i++)
            {
                Image currentImage = new Image
                {
                    Width = 60,
                    Height = 60,
                    Name = "Rim"+i,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Source = new BitmapImage(new Uri(@"/Theme/Assets/ball.png", UriKind.Relative))
                };
                Canvas.SetLeft(currentImage,leftValues[i]); 
                Canvas.SetTop(currentImage,400);
                Panel.SetZIndex(currentImage,11);
                BallLocationList.Add(currentImage);
            }


        }
    }
}