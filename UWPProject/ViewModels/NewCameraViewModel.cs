using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class NewCameraViewModel : NotificationBase<Camera>
    {
        private readonly CamerasModel _model;

        public NewCameraViewModel()
        {
            _model = new CamerasModel();
        }

        public string CameraCountry
        {
            get => This.Country;
            set
            {
                SetProperty(This.Country, value, () => This.Country = value);
            }
        }

        public string CameraCity
        {
            get => This.City;
            set { SetProperty(This.City, value, () => This.City = value); }
        }

        public string RtspAddress
        {
            get => This.RtspAddress;
            set { SetProperty(This.RtspAddress, value, () => This.RtspAddress = value); }
        }

        public double CameraLatitude
        {
            get => This.Latitude;
            set { SetProperty(This.Latitude, value, () => This.Latitude = value); }
        }

        public double CameraLongitude
        {
            get => This.Longitude;
            set { SetProperty(This.Longitude, value, () => This.Longitude = value); }
        }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public async Task AddNewCamera()
        {
            double latitude, longitude;

            if (!string.IsNullOrWhiteSpace(Latitude) && double.TryParse(Latitude, out latitude))
            {
                CameraLatitude = latitude;
            }
            else
            {
                await _model.GetLatitude(This);
            }

            if (!string.IsNullOrWhiteSpace(Longitude) && double.TryParse(Longitude, out longitude))
            {
                CameraLongitude = longitude;
            }
            else
            {
                await _model.GetLongitude(This);
            }

            _model.AddCamera(This);
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(This.RtspAddress) || string.IsNullOrWhiteSpace(This.Country) 
                || string.IsNullOrWhiteSpace(This.City))
            {
                return false;
            }

            if (!This.RtspAddress.StartsWith("rtsp://") || This.RtspAddress.Length < 15)
            {
                return false;
            }

            return true;
        }
    }
}
