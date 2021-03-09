using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWPProject.ViewModels
{
    public class CameraViewModel
    {
        private CameraModel _cameraModel;

        public CameraViewModel(int id)
        {
            _cameraModel = new CameraModel { Country = "USA", City = "New-York" };
        }

        public int Id
        {
            get => _cameraModel.Id;
        }

        public string City
        {
            get => _cameraModel.City;
        }

        public string Country
        {
            get => _cameraModel.Country;
        }

        public string ToStringProperty
        {
            get
            {
                return $"{_cameraModel.City}, {_cameraModel.Country}";
            }
        }
    }
}
