using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.Models;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;

namespace UWPProject.ViewModels
{
    public class MapViewModel
    {
        private readonly CamerasModel _model;

        public ObservableCollection<MapLayer> LandmarkLayers { get; }

        public MapViewModel()
        {
            _model = new CamerasModel();
            LandmarkLayers = new ObservableCollection<MapLayer>();

            this.AddCamerasLayer();
        }

        private void AddCamerasLayer()
        {
            var cameraMarks = new List<MapElement>();
            var cameras = _model.GetAllCameras();

            foreach (var camera in cameras)
            {
                BasicGeoposition cameraPosition = new BasicGeoposition
                {
                    Latitude = camera.Latitude,
                    Longitude = camera.Longtitude
                };
                Geopoint cameraPoint = new Geopoint(cameraPosition);

                var icon = new MapIcon
                {
                    Location = cameraPoint,
                    Tag = camera.Id,
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    ZIndex = 0,
                    Title = $"{camera.City}, {camera.Country}",
                    CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                };

                cameraMarks.Add(icon);
            }

            var camerasLayer = new MapElementsLayer()
            {
                ZIndex = 1,
                MapElements = cameraMarks,
            };
            /////////camerasLayer.MapElementClick += AA;

            LandmarkLayers.Add(camerasLayer);
        }

        private void AA(object sender, MapElementsLayerClickEventArgs args)
        {
            var a = sender;
            var b = args.MapElements[0];
            int o = 0;
        }
    }
}
