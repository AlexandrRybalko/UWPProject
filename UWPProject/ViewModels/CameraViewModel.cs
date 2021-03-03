using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class CameraViewModel
    {
        private CameraModel _camera;

        public CameraModel Camera {
            get
            {
                return this._camera;
            } 
        }

        public int Id
        {
            get => _camera.Id;
        }

        public string City
        {
            get => _camera.City;
        }

        public string Country
        {
            get => _camera.Country;
        }

        public CameraViewModel(int id)
        {
            _camera = new CameraModel { Id =  id, Country = "USA", City = "New-York" };
        }

        public string ToStringProperty
        {
            get
            {
                return $"{_camera.City}, {_camera.Country}";
            }
        }
    }
}
