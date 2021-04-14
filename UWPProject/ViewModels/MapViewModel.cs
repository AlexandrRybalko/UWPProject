using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UWPProject.Models;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;

namespace UWPProject.ViewModels
{
    public class MapViewModel : System.IDisposable
    {
        private readonly CamerasModel model;

        public ObservableCollection<MapLayer> LandmarkLayers { get; }

        public MapViewModel()
        {
            model = new CamerasModel();
            LandmarkLayers = new ObservableCollection<MapLayer>();

            this.AddCamerasLayer();
        }

        private void AddCamerasLayer()
        {
            var cameraMarks = new List<MapElement>();
            var cameras = model.GetAllCameras();

            foreach (var camera in cameras)
            {
                BasicGeoposition cameraPosition = new BasicGeoposition
                {
                    Latitude = camera.Latitude,
                    Longitude = camera.Longitude
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

            LandmarkLayers.Add(camerasLayer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                model.Dispose();
            }
        }
    }
}
