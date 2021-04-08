using System;
using UWPProject.Models;
using UWPProject.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ResourceLoader _resourceLoader;
        private NewCameraViewModel _newCamera;

        public CamerasViewModel CamerasViewModel { get; set; }
        public ButtonCommand AddNewCameraCommand { get; set; }
        public bool CanGoBack { get => (Window.Current.Content as Frame).CanGoBack; }

        public MainPage()
        {
            this.InitializeComponent();
            CamerasViewModel = new CamerasViewModel();
            _newCamera = new NewCameraViewModel();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;
            if (!string.IsNullOrEmpty(language))
            {
                this._resourceLoader = ResourceLoader.GetForCurrentView(language);
            }
            else
            {
                this._resourceLoader = ResourceLoader.GetForCurrentView("En-en");
            }

            AddNewCameraCommand = new ButtonCommand(new Action(AddCamera), () => false);
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

        private void SearchBox_QueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
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
            this.Frame.GoBack();
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            Done.Command = new ButtonCommand(new Action(AddCamera), () => _newCamera.IsValid());
        }

        private async void AddCamera()
        {
            await _newCamera.AddNewCamera();
            CamerasViewModel.UpdateCameras();
            HideFlyout();
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
            dialog.Content = _resourceLoader.GetString("CameraHasBeenAdded");
            dialog.CloseButtonText = "OK";
            dialog.CloseButtonStyle = (Style)this.Resources["buttonStyle"];

            await dialog.ShowAsync();            
        }
    }
}
