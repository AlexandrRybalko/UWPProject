using UWPProject.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace UWPProject
{
    public sealed partial class MapPage : Page
    {
        private MapViewModel viewModel;
        private CamerasViewModel camerasViewModel;
        private ResourceLoader resourceLoader;

        public ButtonCommand GoBackCommand { get; set; }

        public MapPage()
        {
            this.InitializeComponent();

            viewModel = new MapViewModel();
            camerasViewModel = new CamerasViewModel();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;
            resourceLoader = ResourceLoader.GetForCurrentView(language);

            GoBackCommand = new ButtonCommand(new System.Action(GoBack));
        }

        private void CameraMap_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MapElementsLayer a = this.CameraMap.Layers[0] as MapElementsLayer;
            a.MapElementClick += NavigateToCameraPage;
        }

        private void NavigateToCameraPage(MapElementsLayer sender, MapElementsLayerClickEventArgs args)
        {
            int id = (int)args.MapElements[0].Tag;

            var camera = this.camerasViewModel.GetById(id);
            this.Frame.Navigate(typeof(CameraPage), camera);
        }

        private void GoBack()
        {
            this.Frame.GoBack();
        }
    }
}
