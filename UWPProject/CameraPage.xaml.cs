using System;
using UWPProject.Entities;
using UWPProject.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWPProject
{
    public partial class CameraPage : Page
    {
        private FFmpegInterop.FFmpegInteropMSS ffmpeg;
        private ResourceLoader resourceLoader;

        public CameraViewModel CameraViewModel { get; set; }
        public ButtonCommand GoBackCommand { get; set; }
        public ButtonCommand AddToFavouritesCommand { get; set; }
        public ButtonCommand RemoveFromFavouritesCommand { get; set; }

        public CameraPage()
        {
            InitializeComponent();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;
            resourceLoader = ResourceLoader.GetForCurrentView(language);
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
            catch(System.Runtime.InteropServices.COMException exception)
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
    }
}
