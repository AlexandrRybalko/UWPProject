using System;
using UWPProject.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWPProject
{
    public sealed partial class UnknownCameraPage : Page
    {
        private string rtspAddress;
        private FFmpegInterop.FFmpegInteropMSS ffmpeg;
        private ResourceLoader resourceLoader;

        public UnknownCameraViewModel UnknownCameraViewModel { get; set; }
        public ButtonCommand AddNewCameraCommand { get; set; }
        public ButtonCommand NavigateToMainPageCommand { get; }

        public UnknownCameraPage()
        {
            this.InitializeComponent();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;
            if (!string.IsNullOrEmpty(language))
            {
                this.resourceLoader = ResourceLoader.GetForCurrentView(language);
            }
            else
            {
                this.resourceLoader = ResourceLoader.GetForCurrentView("En-en");
            }

            this.UnknownCameraViewModel = new UnknownCameraViewModel();

            AddNewCameraCommand = new ButtonCommand(new Action(AddCamera), () => false);
            NavigateToMainPageCommand = new ButtonCommand(new Action(() => this.Frame.Navigate(typeof(MainPage))));
        }        

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                rtspAddress = e.Parameter as string;
                UnknownCameraViewModel.RtspAddress = rtspAddress;
                this.ffmpeg = await FFmpegInterop.FFmpegInteropMSS.CreateFromUriAsync(rtspAddress);
                MediaStreamSource streamSource = ffmpeg.GetMediaStreamSource();
                this.MediaElement.SetMediaStreamSource(streamSource);
                this.MediaElement.Play();
            }
            catch (Exception ex)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Content = ex.Message;
                dialog.CloseButtonText = "OK";
                dialog.CloseButtonStyle = (Style)this.Resources["buttonStyle"];

                await dialog.ShowAsync();
            }
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            Done.Command = new ButtonCommand(new Action(AddCamera), () => UnknownCameraViewModel.IsValid());
        }

        public async void AddCamera()
        {
            await UnknownCameraViewModel.AddNewCamera();

            AddCameraFlyout.Hide();

            Country.Text = "";
            City.Text = "";
            Latitude.Text = "0.0";
            Longitude.Text = "0.0";

            AddButtonIcon.Glyph = "\xE73E";
            AddButton.IsEnabled = false;

            ContentDialog dialog = new ContentDialog();
            dialog.Content = resourceLoader.GetString("CameraHasBeenAdded");
            dialog.CloseButtonText = "OK";

            await dialog.ShowAsync();
        }
    }
}
