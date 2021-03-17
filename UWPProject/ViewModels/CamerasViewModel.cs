using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

            foreach (var camera in _model.GetAll())
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

        public void AddCamera(Camera camera)
        {
            _model.AddCamera(camera);
            UpdateCameras();

        }

        public Camera GetById(int id)
        {
            return _model.GetById(id);
        }

        private void UpdateCameras()
        {
            _cameras.Clear();

            foreach (var camera in _model.GetAll())
            {
                var cameraViewModel = new CameraViewModel(camera);
                _cameras.Add(cameraViewModel);
            }
        }
    }
}
