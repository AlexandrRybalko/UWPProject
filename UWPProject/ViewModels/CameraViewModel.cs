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
    public class CameraViewModel : NotificationBase<Camera>, IDisposable
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
            return !model.GetFavourites().Any(x => x.Id == this.Entity.Id);
        }

        public int Id
        {
            get { return Entity.Id; }
        }

        public string RtspAddress
        {
            get { return Entity.RtspAddress; }
            set { SetProperty(Entity.RtspAddress, value, () => Entity.RtspAddress = value); }
        }

        public string Country
        {
            get { return Entity.Country; }
            set { SetProperty(Entity.Country, value, () => Entity.Country = value); }
        }

        public string City
        {
            get { return Entity.City; }
            set { SetProperty(Entity.City, value, () => Entity.City = value); }
        }

        public double Latitude 
        { 
            get { return Entity.Latitude; } 
        }

        public double Longitude 
        {
            get { return Entity.Longitude; } 
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
            model.AddToCategory(Entity.Id, "Favourites");
        }

        public void AddToRecent()
        {
            model.AddToCategory(Entity.Id, "Recent");
        }

        public void RemoveFromFavourites()
        {
            model.RemoveFromCategory(Entity.Id, "Favourites");
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                model.Dispose();
            }
        }
    }
}
