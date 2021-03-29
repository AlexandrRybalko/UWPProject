using System;
using UWPProject.Models;
using UWPProject.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
//using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ResourceLoader _resourceLoader;
        public CamerasViewModel CamerasViewModel { get; set; } 
        public ButtonCommand AddNewCameraCommand { get; }

        public MainPage()
        {
            this.InitializeComponent();
            CamerasViewModel = new CamerasViewModel();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;
            if (!string.IsNullOrEmpty(language))
            {
                this._resourceLoader = ResourceLoader.GetForCurrentView(language);
            }
            else
            {
                this._resourceLoader = ResourceLoader.GetForCurrentView("En-en");
            }

            AddNewCameraCommand = new ButtonCommand(new Action(AddNewCamera));        
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            Navigation.SelectedItem = this.Random;
        }

        private void AddNewCamera()
        {
            if (!(string.IsNullOrEmpty(this.Country.Text) || string.IsNullOrEmpty(this.City.Text) ||
                string.IsNullOrEmpty(this.RtspAddressTextBox.Text)))
            {
                Camera camera = new Camera
                {
                    Country = this.Country.Text,
                    City = this.City.Text,
                    RtspAddress = this.RtspAddressTextBox.Text,
                };

                this.CamerasViewModel.AddCamera(camera);
            }
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
                string categoryName = args.SelectedItemContainer.Content.ToString();
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

        private void HideFlyout(object sender, RoutedEventArgs e)
        {
            AddCameraFlyout.Hide();
        }
    }
}
