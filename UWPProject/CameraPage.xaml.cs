using MjpegProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPProject.Models;
using UWPProject.ViewModels;
using Windows.Media.Core;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace UWPProject
{
    public partial class CameraPage : Page
    {
        private FFmpegInterop.FFmpegInteropMSS _ffmpeg;

        public CameraViewModel CameraViewModel { get; set; }
        public ButtonCommand GoBackCommand { get; set; }
        public ButtonCommand AddToFavouritesCommand { get; set; }
        public ButtonCommand RemoveFromFavouritesCommand { get; set; }

        public CameraPage()
        {
            InitializeComponent();                        
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Camera camera = e.Parameter as Camera;
            this.CameraViewModel = new CameraViewModel(camera);

            this.GoBackCommand = new ButtonCommand(new Action(GoBack));
            this.AddToFavouritesCommand = new ButtonCommand(new Action(this.AddToFavourites), () => true);
            this.RemoveFromFavouritesCommand = new ButtonCommand(new Action(this.RemoveFromFavourites), () => true);

            if (this.CameraViewModel.CanExecuteAddToFavouritesCommand())
            {
                this.AddToFavouritesButton.Command = this.AddToFavouritesCommand;
            }
            else
            {
                this.AddToFavouritesButton.Command = this.RemoveFromFavouritesCommand;
            }

            this._ffmpeg = await FFmpegInterop.FFmpegInteropMSS.CreateFromUriAsync(camera.RtspAddress);
            MediaStreamSource streamSource = _ffmpeg.GetMediaStreamSource();
            this.MediaElement.SetMediaStreamSource(streamSource);
            this.MediaElement.Play();

            CameraViewModel.AddToRecent();
        }

        private void GoBack()
        {
            //this.Frame.GoBack();

            this.Frame.Navigate(typeof(UnknownCameraPage), CameraViewModel.RtspAddress);
        }

        private void AddToFavourites()
        {
            this.CameraViewModel.AddToFavourites();
            /*this.AddToFavouritesButton.Command = RemoveFromFavouritesCommand;
            this.AddToFavouritesButton.Style = (Style)this.Resources["removeFromFavouritesButton"];*/
        }

        private void RemoveFromFavourites()
        {
            this.CameraViewModel.RemoveFromFavourites();
            this.AddToFavouritesButton.Command = this.AddToFavouritesCommand;
            this.AddToFavouritesButton.Style = (Style)this.Resources["addToFavouritesButton"];
        }
    }
}
