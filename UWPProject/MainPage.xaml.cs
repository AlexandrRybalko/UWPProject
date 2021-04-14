using BackgroundTaskRuntimeComponent;
using MjpegProcessor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UWPProject.Entities;
using UWPProject.ViewModels;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IDisposable
    {
        private ResourceLoader resourceLoader;
        private NewCameraViewModel newCamera;
        private FFmpegInterop.FFmpegInteropMSS ffmpeg;

        public CamerasViewModel CamerasViewModel { get; set; }
        public ButtonCommand AddNewCameraCommand { get; set; }
        public ButtonCommand RefreshCamerasCommand { get; set; }
        public bool CanGoBack { get => (Window.Current.Content as Frame).CanGoBack; }

        public MainPage()
        {
            this.InitializeComponent();
            CamerasViewModel = new CamerasViewModel();
            newCamera = new NewCameraViewModel();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;
            if (!string.IsNullOrEmpty(language))
            {
                this.resourceLoader = ResourceLoader.GetForCurrentView(language);
            }
            else
            {
                this.resourceLoader = ResourceLoader.GetForCurrentView("En-en");
                ApplicationData.Current.LocalSettings.Values["Language"] = "En-en";
            }

            AddNewCameraCommand = new ButtonCommand(new Action(AddCamera), () => false);
            RefreshCamerasCommand = new ButtonCommand(new Action(StartBackgroundTask));
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            Navigation.SelectedItem = this.Random;
            Navigation.IsBackButtonVisible = (CanGoBack) ? NavigationViewBackButtonVisible.Visible : NavigationViewBackButtonVisible.Collapsed;
        }

        private void NavigateToCameraPage(object sender, RoutedEventArgs e)
        {
            var camera = this.GridView.SelectedItem;
            var type = this.GridView.SelectedItem.GetType();
            int id = (int)type.GetProperty("Id").GetValue(camera);
            Camera c = this.CamerasViewModel.GetById(id);
            this.Frame.Navigate(typeof(CameraPage), c);
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (args.IsSettingsSelected)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
            else
            {
                string categoryName = args.SelectedItemContainer.Name.ToString();
                CamerasViewModel.GetByCategory(categoryName);
            }            
        }

        private async void StartBackgroundTask()
        {
            var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(x => x.Name == "testTask");
            if (task != null)
            {
                task.Unregister(true);
            }

            var taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = "testTask";
            taskBuilder.TaskEntryPoint = typeof(UpdateLocalDbBackgroundTask).ToString();

            ApplicationTrigger trigger = new ApplicationTrigger();

            taskBuilder.SetTrigger(trigger);
            taskBuilder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            task = taskBuilder.Register();
            //task.Completed += (BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args) => CamerasViewModel.UpdateCameras();

            await trigger.RequestAsync();
        }

        private void SearchBox_QueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            var selectedItem = this.Navigation.SelectedItem as NavigationViewItem;

            if (selectedItem.Content.Equals("Recent"))
            {
                CamerasViewModel.SelectedCategory = Enums.Category.Recent;
                CamerasViewModel.SearchRecentCameras(args.QueryText);
            }
            else if (selectedItem.Content.Equals("Favourites"))
            {
                CamerasViewModel.SelectedCategory = Enums.Category.Favourite;
                CamerasViewModel.SearchFavouriteCameras(args.QueryText);
            }
            else
            {
                CamerasViewModel.SelectedCategory = 0;
                CamerasViewModel.SearchRandomCameras(args.QueryText);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapPage));
        }

        private void Navigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            this.Frame.GoBack();
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            Done.Command = new ButtonCommand(new Action(AddCamera), () => newCamera.IsValid());
        }

        private async void AddCamera()
        {
            try
            {
                ffmpeg = await FFmpegInterop.FFmpegInteropMSS.CreateFromUriAsync(RtspAddressTextBox.Text);
                await newCamera.AddNewCamera();
                CamerasViewModel.UpdateCameras();
                HideFlyout();
            }
            catch(System.Runtime.InteropServices.COMException)
            {
                ShowContentDialog("Can not add this camera");
            }
        }

        private async void HideFlyout()
        {
            AddCameraFlyout.Hide();
            Country.Text = "";
            City.Text = "";
            RtspAddressTextBox.Text = "";
            Latitude.Text = "0.0";
            Longitude.Text = "0.0";

            ContentDialog dialog = new ContentDialog();
            dialog.Content = resourceLoader.GetString("CameraHasBeenAdded");
            dialog.CloseButtonText = "OK";
            dialog.CloseButtonStyle = (Style)this.Resources["buttonStyle"];

            await dialog.ShowAsync();            
        }

        private static async void ShowContentDialog(string message)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Content = message;
            dialog.CloseButtonText = "OK";

            await dialog.ShowAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                newCamera.Dispose();
            }
        }
    }
}
