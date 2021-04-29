//using libVLCX;
using System;
using System.IO;
using UWPProject.Entities;
using UWPProject.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using LibVLCSharp.Shared;
using Windows.UI.Core;
using Xabe.FFmpeg;

namespace UWPProject
{
    public partial class CameraPage : Page
    {
        private FFmpegInterop.FFmpegInteropMSS ffmpeg;
        private ResourceLoader resourceLoader;
        private bool isRecording;
        private MediaPlayer mediaPlayer;
        private LibVLC libVLC;

        public CameraViewModel CameraViewModel { get; set; }
        public ButtonCommand GoBackCommand { get; set; }
        public ButtonCommand AddToFavouritesCommand { get; set; }
        public ButtonCommand RemoveFromFavouritesCommand { get; set; }

        public CameraPage()
        {
            InitializeComponent();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;
            resourceLoader = ResourceLoader.GetForCurrentView(language);
            isRecording = false;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            base.OnNavigatedTo(e);
            Camera camera = e.Parameter as Camera;
            this.CameraViewModel = new CameraViewModel(camera);

            this.GoBackCommand = new ButtonCommand(new Action(GoBack));
            this.AddToFavouritesCommand = new ButtonCommand(new Action(this.AddToFavourites));
            this.RemoveFromFavouritesCommand = new ButtonCommand(new Action(this.RemoveFromFavourites));

            if (this.CameraViewModel.CanExecuteAddToFavouritesCommand())
            {
                this.AddToFavouritesButton.Command = this.AddToFavouritesCommand;
            }
            else
            {
                this.AddToFavouritesButton.Command = this.RemoveFromFavouritesCommand;
                this.HeartIcon.Glyph = "\xEB52";
            }

            try
            {
                this.ffmpeg = await FFmpegInterop.FFmpegInteropMSS.CreateFromUriAsync(camera.RtspAddress);
                MediaStreamSource streamSource = ffmpeg.GetMediaStreamSource();

                this.MediaElement.SetMediaStreamSource(streamSource);
                this.MediaElement.Play();

                CameraViewModel.AddToRecent();
            }
            catch (System.Runtime.InteropServices.COMException exception)
            {
                ShowContentDialog(exception.Message);
                GoBack();
            }
        }

        private void GoBack()
        {
            this.Frame.GoBack();
        }

        private void AddToFavourites()
        {
            this.CameraViewModel.AddToFavourites();
            this.HeartIcon.Glyph = "\xEB52";
            ShowContentDialog(resourceLoader.GetString("CameraHasBeenAddedToFavourites"));

            this.AddToFavouritesButton.Command = new ButtonCommand(new Action(RemoveFromFavourites));
        }

        private void RemoveFromFavourites()
        {
            this.CameraViewModel.RemoveFromFavourites();
            this.HeartIcon.Glyph = "\xEB51";
            ShowContentDialog(resourceLoader.GetString("CameraHasBeenRemovedFromFavourites"));

            this.AddToFavouritesButton.Command = new ButtonCommand(new Action(AddToFavourites));
        }

        private static async void ShowContentDialog(string message)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Content = message;
            dialog.CloseButtonText = "OK";

            await dialog.ShowAsync();
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var currentDirectory = ApplicationData.Current.LocalFolder.Path;
            var destination = Path.Combine(currentDirectory, $"{this.CameraViewModel.City}_{this.CameraViewModel.Country}.mp4");
            isRecording = !isRecording;
            if (isRecording)
            {
                // Record in a file "record.ts" located in the bin folder next to the app
                
                // Load native libvlc library
                Core.Initialize();

                libVLC = new LibVLC();
                mediaPlayer = new MediaPlayer(libVLC);

                libVLC.Log += (s, v) => Console.WriteLine($"[{v.Level}] {v.Module}:{v.Message}");

                // Create new media with HLS link
                using (var media = new Media(libVLC, new Uri(this.CameraViewModel.RtspAddress),
                    // Define stream output options.
                    // In this case stream to a file with the given path and play locally the stream while streaming it.
                    ":sout=#file{dst=" + destination + "}",
                    ":sout-keep"))
                {
                    // Start recording
                    this.mediaPlayer.Play(media);
                }
            }
            else
            {
                this.mediaPlayer.Stop();
                mediaPlayer.Dispose();
                libVLC.Dispose();

                //await Conversion.Convert(destination, currentDirectory + @"\file.mp4").Start();
            }            
        }
    }
}
