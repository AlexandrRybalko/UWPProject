using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class CamerasViewModel : NotificationBase
    {
        private readonly CamerasModel _model;
        private ObservableCollection<CameraViewModel> _cameras;

        public CamerasViewModel()
        {
            _model = new CamerasModel();
            _cameras = new ObservableCollection<CameraViewModel>();

            var cameras = _model.GetRandom();
            foreach (var camera in cameras)
            {
                var cameraViewModel = new CameraViewModel(camera);
                _cameras.Add(cameraViewModel);
            }
        }

        public ObservableCollection<CameraViewModel> Cameras
        {
            get { return _cameras; }
            set { SetProperty(ref _cameras, value); }
        }

        public Enums.Category SelectedCategory { get; set; }

        public void AddCamera(Camera camera)
        {
            _model.AddCamera(camera);
            UpdateCameras(_model.GetRandom());
        }

        public void GetByCategory(string categoryName)
        {
            UpdateCameras(_model.GetCamerasByCategory(categoryName));
        }

        public Camera GetById(int id)
        {
            return _model.GetById(id);
        }

        public void SearchRandomCameras(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateCameras(_model.GetRandom());
                return;
            }

            _cameras.Clear();

            foreach (var camera in _model.GetAllCameras().Where(x => x.City.Contains(query) || x.Country.Contains(query)))
            {
                var cameraViewModel = new CameraViewModel(camera);
                _cameras.Add(cameraViewModel);
            }
        }

        public void SearchFavouriteCameras(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateCameras(_model.GetFavourites());
                return;
            }

            _cameras.Clear();
            var searchedCameras = _model.GetFavourites();

            foreach (var camera in searchedCameras.Where(x => x.City.Contains(query) || x.Country.Contains(query)))
            {
                var cameraViewModel = new CameraViewModel(camera);
                _cameras.Add(cameraViewModel);
            }
        }

        public void SearchRecentCameras(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                UpdateCameras(_model.GetRecent());
                return;
            }

            _cameras.Clear();
            var searchedCameras = _model.GetRecent();

            foreach (var camera in searchedCameras.Where(x => x.City.Contains(query) || x.Country.Contains(query)))
            {
                var cameraViewModel = new CameraViewModel(camera);
                _cameras.Add(cameraViewModel);
            }
        }

        private void UpdateCameras(IEnumerable<Camera> cameras)
        {
            _cameras.Clear();

            foreach (var camera in cameras)
            {
                var cameraViewModel = new CameraViewModel(camera);
                _cameras.Add(cameraViewModel);
            }
        }

        public void UpdateCameras()
        {
            _cameras.Clear();

            var cameras = _model.GetAllCameras();

            foreach (var camera in cameras)
            {
                var cameraViewModel = new CameraViewModel(camera);
                _cameras.Add(cameraViewModel);
            }
        }

        public async Task GetLatitude(Camera camera)
        {
            await _model.GetLatitude(camera);
        }

        public async Task GetLongitude(Camera camera)
        {
             await _model.GetLongitude(camera);
        }
    }
}
