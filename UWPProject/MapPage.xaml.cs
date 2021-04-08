using UWPProject.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace UWPProject
{
    public sealed partial class MapPage : Page
    {
        private MapViewModel _viewModel;
        private CamerasViewModel _camerasViewModel;

        public MapPage()
        {
            this.InitializeComponent();
            _viewModel = new MapViewModel();
            _camerasViewModel = new CamerasViewModel();
        }

        private void CameraMap_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MapElementsLayer a = this.CameraMap.Layers[0] as MapElementsLayer;
            a.MapElementClick += NavigateToCameraPage;
        }

        private void NavigateToCameraPage(MapElementsLayer sender, MapElementsLayerClickEventArgs args)
        {
            int id = (int)args.MapElements[0].Tag;

            var camera = this._camerasViewModel.GetById(id);
            this.Frame.Navigate(typeof(CameraPage), camera);
        }
    }
}
