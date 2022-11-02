using Machina.model;
using Machina.Service;
using Plugin.Media.Abstractions;
using Plugin.SimpleAudioPlayer;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Machina
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPage : ContentPage
    {
        bool processing = true;
        FaceDetectResult faceDetectResult = null;
        public ScannerPage( MediaFile file)
        {
            
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this,false);
            faceImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStreamWithImageRotatedForExternalStorage();
                return stream;
            });

            _ = startlaserAnimation();
            _ = startDetection(file);
            
           
        }
        private async Task startlaserAnimation()
        {
            laserImage.Opacity = 0;
            await Task.Delay(500);
            await laserImage.FadeTo(1, 500);
            PlaySound("scan.wav");
            await laserImage.TranslateTo(0, 360, 1800);
            double y = 0;
            while (processing)
            {
                PlayCurrentSound();
                await laserImage.TranslateTo(0,y, 1800);
                y = (y == 0) ? 360 : 0;
            }
            laserImage.IsVisible = false;
            PlaySound("result.wav");
            await DisplayResult();
        }
        private async Task DisplayResult()
        {
            statusLabel.Text = "Analyse terminée";
            if (faceDetectResult == null)
            {
                //faceLabel.Text = "Erreur : Pas de vissage detecté";
                await DisplayAlert("Erreur", "L'analyse n'a pas fonctionné", "ok");
                await Navigation.PopAsync();
            }
            else
            {
                GenderLabel.Text = faceDetectResult.faceAttributes.gender.Substring(0, 1).ToUpper();
                AgeLabel.Text = faceDetectResult.faceAttributes.age.ToString();
                infoLayout.IsVisible = true;
                continueButton.IsVisible = true;

            }

        }
        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private async Task startDetection(MediaFile file)
        {
            
            faceDetectResult =  await CognitiveService.FaceDetect(file.GetStreamWithImageRotatedForExternalStorage());
            processing = false;
           
        }
        private void PlaySound(string soundName)
        {
            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(GetStreamFromFile(soundName));
            player.Play();  
        }
        private void PlayCurrentSound()
        {
            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Stop();
            player.Play();  
        }

        private Stream GetStreamFromFile(FileImageSource filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("Machina." + filename);
            return stream;
        }
    }
}