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
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace UWPProject
{
    public partial class CameraPage : Page
    {
        public CameraViewModel CameraViewModel { get; set; }
        private DispatcherTimer _timer;
        private MjpegDecoder _mjpegDecoder;

        public CameraPage()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Camera camera = e.Parameter as Camera;
            CameraViewModel = new CameraViewModel(camera);

            var goBackCommand = new StandardUICommand(StandardUICommandKind.None);
            goBackCommand.ExecuteRequested += GoBack;
            this.BackButton.Command = goBackCommand;

            if (CameraViewModel.ImageType.Equals("mjpg"))
            {
                _mjpegDecoder = new MjpegDecoder();
                _mjpegDecoder.FrameReady += mjpeg_FrameReady;
                _mjpegDecoder.ParseStream(new Uri(CameraViewModel.IpAddress));
            }
            else if (CameraViewModel.ImageType.Equals("jpg"))
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(10);
                _timer.Tick += GetImage;
                _timer.Start();           
            
            }
        }

        private async void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                await ms.WriteAsync(e.FrameBuffer);
                ms.Seek(0);

                var bmp = new BitmapImage();
                await bmp.SetSourceAsync(ms);

                //image is the Image control in XAML
                CameraImage.Source = bmp;
            }
        }        

        private async void GetImage<EventArgs>(object sender, EventArgs e)
        {
            _timer.Stop();
            HttpClient client = new HttpClient();
            Uri requestUri = new Uri(CameraViewModel.IpAddress);
            HttpResponseMessage response = await client.GetAsync(requestUri);
            // A memory stream where write the image data
            InMemoryRandomAccessStream randomAccess = new InMemoryRandomAccessStream();

            DataWriter writer = new DataWriter(randomAccess.GetOutputStreamAt(0));

            // Write and save the data into the stream
            writer.WriteBytes(await response.Content.ReadAsByteArrayAsync());
            await writer.StoreAsync();

            // Create a Bitmap and assign it to the target Image control
            BitmapImage bm = new BitmapImage();
            await bm.SetSourceAsync(randomAccess);
            CameraImage.Source = bm;
            _timer.Start();
        }

        private void GoBack(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            this.Frame.GoBack();
        }
    }
}
