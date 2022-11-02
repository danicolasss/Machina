using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Machina
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }
        
        private void StartButton_Clicked(object sender, EventArgs e)
        {
            _ = StartButton_ClickedAsync();
        }
        private async Task StartButton_ClickedAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("ERREUR : ", "La caméra n'est pas disponible", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
            });

            if (file == null)
            {
                await DisplayAlert("ERREUR : ", "Pas de fichier", "OK");
                return;
            }
               

            await DisplayAlert("Chemin du fichier : ", file.Path, "OK");

            /*image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });*/
            await Navigation.PushAsync(new ScannerPage(file));
        }
    }
}
