using MjpegProcessor;
using System;
using System.Linq;
using UWPProject.Entities;
using UWPProject.Models;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPProject.ViewModels
{
    public class CameraViewModel : NotificationBase<Camera>
    {
        private readonly CamerasModel model = new CamerasModel();
        private MjpegDecoder mjpegDecoder;
        private ImageSource previewImage = new BitmapImage();

        public CameraViewModel(Camera camera = null) : base(camera) 
        {
            AddToFavouritesCommand = new ButtonCommand(new Action(AddToFavourites), CanExecuteAddToFavouritesCommand);

            mjpegDecoder = new MjpegDecoder();
            mjpegDecoder.FrameReady += FrameReady;
            mjpegDecoder.ParseStream(new Uri($"http://{Ip}/mjpg/video.mjpg"));
        }

        public ButtonCommand AddToFavouritesCommand { get; }

        public bool CanExecuteAddToFavouritesCommand()
        {
            return !model.GetFavourites().Any(x => x.Id == this.This.Id);
        }

        public int Id
        {
            get { return This.Id; }
        }

        public string RtspAddress
        {
            get { return This.RtspAddress; }
            set { SetProperty(This.RtspAddress, value, () => This.RtspAddress = value); }
        }

        public string Country
        {
            get { return This.Country; }
            set { SetProperty(This.Country, value, () => This.Country = value); }
        }

        public string City
        {
            get { return This.City; }
            set { SetProperty(This.City, value, () => This.City = value); }
        }

        public double Latitude 
        { 
            get { return This.Latitude; } 
        }

        public double Longitude 
        {
            get { return This.Longitude; } 
        }

        public string ToStringProperty
        {
            get { return $"{this.Country}, {this.City}"; }
        }

        private string Ip
        {
            get => RtspAddress.Split('/')[2];
        }

        public ImageSource PreviewImage
        {
            get => previewImage;
            set { SetProperty<ImageSource>(previewImage, value, () => previewImage = value); }
        }

        public void AddToFavourites()
        {
            model.AddToCategory(This.Id, "Favourites");
        }

        public void AddToRecent()
        {
            model.AddToCategory(This.Id, "Recent");
        }

        public void RemoveFromFavourites()
        {
            model.RemoveFromCategory(This.Id, "Favourites");
        }        

        private async void FrameReady(object sender, FrameReadyEventArgs e)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(e.FrameBuffer);
                stream.Seek(0);

                var bmp = new BitmapImage();
                await bmp.SetSourceAsync(stream);

                PreviewImage = bmp;
            }

            try
            {
                mjpegDecoder.FrameReady -= FrameReady;
                mjpegDecoder = null;
            }
            catch (System.ArgumentException)
            {

            }
        }
    }
}
