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
        public CameraViewModel Cameras { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Cameras = new CameraViewModel();
            this.SetLanguage("Ru-ru");
        }

        private void SetLanguage(string language, object e = null)
        {
            this._resourceLoader = ResourceLoader.GetForCurrentView(language);
            this.Random.Content = _resourceLoader.GetString("Random");
            this.TopRated.Content = _resourceLoader.GetString("TopRated");
            this.Recent.Content = _resourceLoader.GetString("Recent");
            this.Favourites.Content = _resourceLoader.GetString("Favourites");
            this.Language.Header = _resourceLoader.GetString("Language");
            this.Language.SelectedIndex = 0;
            this.Russian.Content = _resourceLoader.GetString("Russian");
            this.English.Content = _resourceLoader.GetString("English");
        }
    }
}
