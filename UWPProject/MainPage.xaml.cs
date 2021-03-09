using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public CamerasListViewModel Cameras { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Cameras = new CamerasListViewModel();
            this.SetEnglishLanguage(null, null);            
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            var setEnglish = new StandardUICommand(StandardUICommandKind.None);
            setEnglish.ExecuteRequested += SetEnglishLanguage;
            this.English.Command = setEnglish;

            var setRussian = new StandardUICommand(StandardUICommandKind.None);
            setRussian.ExecuteRequested += SetRussianLanguage;
            this.Russian.Command = setRussian;

            this.GridView.SelectionChanged += NavigateToCameraPage;
        }

        private void SetEnglishLanguage(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            this._resourceLoader = ResourceLoader.GetForCurrentView("En-en");
            this.Random.Content = _resourceLoader.GetString("Random");
            this.TopRated.Content = _resourceLoader.GetString("TopRated");
            this.Recent.Content = _resourceLoader.GetString("Recent");
            this.Favourites.Content = _resourceLoader.GetString("Favourites");
            this.LanguageTextBlock.Text = _resourceLoader.GetString("Language");
            this.Russian.Text = _resourceLoader.GetString("Russian");
            this.English.Text = _resourceLoader.GetString("English");
        }

        private void SetRussianLanguage(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            this._resourceLoader = ResourceLoader.GetForCurrentView("Ru-ru");
            this.Random.Content = _resourceLoader.GetString("Random");
            this.TopRated.Content = _resourceLoader.GetString("TopRated");
            this.Recent.Content = _resourceLoader.GetString("Recent");
            this.Favourites.Content = _resourceLoader.GetString("Favourites");
            this.LanguageTextBlock.Text = _resourceLoader.GetString("Language");
            this.Russian.Text = _resourceLoader.GetString("Russian");
            this.English.Text = _resourceLoader.GetString("English");
        }

        private void NavigateToCameraPage(object sender, RoutedEventArgs e)
        {
            var camera = this.GridView.SelectedItem;
            var type = this.GridView.SelectedItem.GetType();
            int id = (int)type.GetProperty("Id").GetValue(camera);
            this.Frame.Navigate(typeof(CameraPage), id);
        }
    }
}
