﻿using DemoLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_XKCD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int maxNumber = 0;
        private int currentNumber = 0;
        private Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
            nextImageButton.IsEnabled = false;
        }

        private async Task LoadImage(int imageNumber = 0)
        {
            var comic = await ComicProcessor.LoadComic(imageNumber);

            if (imageNumber == 0)
            {
                maxNumber = comic.Num;
            }

            currentNumber = comic.Num;

            var uriSource = new Uri(comic.Img, UriKind.Absolute);
            comicImage.Source = new BitmapImage(uriSource);
            comicImage.ToolTip = comic.Alt;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadImage();
        }

        private async void PreviousImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentNumber > 1)
            {
                currentNumber -= 1;
                nextImageButton.IsEnabled = true;
                await LoadImage(currentNumber);

                if (currentNumber == 1)
                {
                    previousImageButton.IsEnabled = false;
                }
            }
        }

        private async void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentNumber < maxNumber)
            {
                currentNumber += 1;
                previousImageButton.IsEnabled = true;
                await LoadImage(currentNumber);

                if (currentNumber == maxNumber)
                {
                    nextImageButton.IsEnabled = false;
                }
            }
        }

        private async void RandomImageButton_Click(object sender, RoutedEventArgs e)
        {
            currentNumber = rnd.Next(1, maxNumber);
            previousImageButton.IsEnabled = true;
            nextImageButton.IsEnabled = true;
            await LoadImage(currentNumber);

            if (currentNumber == 1)
            {
                previousImageButton.IsEnabled = false;
            }

            if (currentNumber == maxNumber)
            {
                nextImageButton.IsEnabled = false;
            }

        }
    }
}
