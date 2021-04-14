using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UWPProject.Entities;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class CamerasViewModel : NotificationBase, System.IDisposable
    {
        private readonly CamerasModel model;
        private ObservableCollection<CameraViewModel> cameras;

        public CamerasViewModel()
        {
            model = new CamerasModel();
            this.cameras = new ObservableCollection<CameraViewModel>();
        }

        public ObservableCollection<CameraViewModel> Cameras
        {
            get { return cameras; }
        }

        public Enums.Category SelectedCategory { get; set; }

        public void AddCamera(Camera camera)
        {
            model.AddCamera(camera);
            UpdateCameras(model.GetRandom());
        }

        public void GetByCategory(string categoryName)
        {
            UpdateCameras(model.GetCamerasByCategory(categoryName));
        }

        public Camera GetById(int id)
        {
            return model.GetById(id);
        }

        public void SearchRandomCameras(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateCameras(model.GetRandom());
                return;
            }

            cameras.Clear();

            foreach (var camera in model.GetAllCameras().Where(x => x.City.Contains(query) || x.Country.Contains(query)))
            {
                var cameraViewModel = new CameraViewModel(camera);
                cameras.Add(cameraViewModel);
            }
        }

        public void SearchFavouriteCameras(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateCameras(model.GetFavourites());
                return;
            }

            cameras.Clear();
            var searchedCameras = model.GetFavourites();

            foreach (var camera in searchedCameras.Where(x => x.City.Contains(query) || x.Country.Contains(query)))
            {
                var cameraViewModel = new CameraViewModel(camera);
                cameras.Add(cameraViewModel);
            }
        }

        public void SearchRecentCameras(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateCameras(model.GetRecent());
                return;
            }

            cameras.Clear();
            var searchedCameras = model.GetRecent();

            foreach (var camera in searchedCameras.Where(x => x.City.Contains(query) || x.Country.Contains(query)))
            {
                var cameraViewModel = new CameraViewModel(camera);
                cameras.Add(cameraViewModel);
            }
        }

        private void UpdateCameras(IEnumerable<Camera> cameras)
        {
            this.cameras.Clear();

            foreach (var camera in cameras)
            {
                var cameraViewModel = new CameraViewModel(camera);
                this.cameras.Add(cameraViewModel);
            }
        }

        public void UpdateCameras()
        {
            this.cameras.Clear();

            var cameras = model.GetRandom();

            foreach (var camera in cameras)
            {
                var cameraViewModel = new CameraViewModel(camera);
                this.cameras.Add(cameraViewModel);
            }
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
