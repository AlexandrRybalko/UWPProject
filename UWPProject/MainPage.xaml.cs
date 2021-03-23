using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPProject.Models;
using UWPProject.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        public MainPage()
        {
            this.InitializeComponent();
            CamerasViewModel = new CamerasViewModel();
            this.SetEnglishLanguage(null, null);            
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            Navigation.SelectedItem = this.Random;

            var setEnglish = new StandardUICommand(StandardUICommandKind.None);
            setEnglish.ExecuteRequested += SetEnglishLanguage;
            this.English.Command = setEnglish;

            var setRussian = new StandardUICommand(StandardUICommandKind.None);
            setRussian.ExecuteRequested += SetRussianLanguage;
            this.Russian.Command = setRussian;

            var addCamera = new StandardUICommand(StandardUICommandKind.None);
            addCamera.ExecuteRequested += AddNewCamera;
            this.Done.Command = addCamera;

            this.GridView.SelectionChanged += NavigateToCameraPage;
            this.Navigation.SelectionChanged += Navigation_SelectionChanged;
        }

        private void SetEnglishLanguage(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            this._resourceLoader = ResourceLoader.GetForCurrentView("En-en");
            this.Random.Content = _resourceLoader.GetString("Random");
            this.Recent.Content = _resourceLoader.GetString("Recent");
            this.Favourites.Content = _resourceLoader.GetString("Favourites");
            this.LanguageTextBlock.Text = _resourceLoader.GetString("Language");
            this.Russian.Text = _resourceLoader.GetString("Russian");
            this.English.Text = _resourceLoader.GetString("English");
            this.AddNewCameraTextBlock.Text = _resourceLoader.GetString("AddNewCameraTextBlock");
            this.SearchBox.PlaceholderText = _resourceLoader.GetString("SearchCamera");
        }

        private void SetRussianLanguage(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            this._resourceLoader = ResourceLoader.GetForCurrentView("Ru-ru");
            this.Random.Content = _resourceLoader.GetString("Random");
            this.Recent.Content = _resourceLoader.GetString("Recent");
            this.Favourites.Content = _resourceLoader.GetString("Favourites");
            this.LanguageTextBlock.Text = _resourceLoader.GetString("Language");
            this.Russian.Text = _resourceLoader.GetString("Russian");
            this.English.Text = _resourceLoader.GetString("English");
            this.AddNewCameraTextBlock.Text = _resourceLoader.GetString("AddNewCameraTextBlock");
            this.SearchBox.PlaceholderText = _resourceLoader.GetString("SearchCamera");
        }

        private void AddNewCamera(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            var b = args;
            if (!(string.IsNullOrEmpty(this.Country.Text) && string.IsNullOrEmpty(this.City.Text) &&
                string.IsNullOrEmpty(this.IpAddress.Text) && string.IsNullOrEmpty(this.ImageType.SelectedItem.ToString())))
            {
                Camera camera = new Camera
                {
                    Country = this.Country.Text,
                    City = this.City.Text,
                    IpAddress = this.IpAddress.Text,
                    ImageType = this.ImageType.SelectedItem.ToString()
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

        private void HideFlyout(object sender, RoutedEventArgs e)
        {
            AddCameraFlyout.Hide();
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string categoryName = args.SelectedItemContainer.Content.ToString();
            CamerasViewModel.GetByCategory(categoryName);
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
    }
}
