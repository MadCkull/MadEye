using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.ViewManagement;
using Microsoft.Graphics.Canvas;
using Windows.Graphics.Imaging;
using CommunityToolkit.WinUI.UI.Controls;
using System;

namespace MadEye
{
    public sealed partial class LoadingScreen : Page
    {

        public LoadingScreen()
        {
            this.InitializeComponent();
            LoadLogoImage();
            Task.Delay(10000);
            SimulateLoading();
        }



        private void LoadLogoImage()
        {
            string imagePath = Path.Combine(AppContext.BaseDirectory, "Assets/LoadScrLogo.png");
            Uri imageUri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            LogoImage.Source = new BitmapImage(imageUri);
        }

        private async void SimulateLoading()
        {
            var LoadingFiles = await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "Resources/FilesList"));
            int LoadedFileCount = 0;

            for (int i = 0; i <= 101; i++)
            {
                progressBar.Value = i;

                if (i == 70)
                {
                    LoadingLabel.Text = GetSlogan();
                }

                if (i < 80)
                {
                    LoadFile.Text = LoadingFiles[LoadedFileCount++];
                }



                      await Task.Delay(60);



                if (i < 80)
                {
                    LoadFile.Text = LoadingFiles[LoadedFileCount++];
                }

                if (i == 90)
                {
                    LoadFile.Text = "Almost Completed";
                }
            }

            LoadFile.Text = "Done!";
            progressBar.ShowPaused = true;
        }


        private string GetSlogan()
        {
            Random random = new Random();
            var slogans = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Resources/Slogans"));
            var randomIndex = random.Next(0, slogans.Length);
            var randomSlogan = slogans[randomIndex];
            return randomSlogan;
        }
    }
}
