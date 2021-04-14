using System;
using System.Threading.Tasks;
using UWPProject.Entities;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class UnknownCameraViewModel : NotificationBase<Camera>, IDisposable
    {
        private readonly CamerasModel model;

        public UnknownCameraViewModel()
        {
            model = new CamerasModel();
        }

        public string CameraCountry 
        {
            get => Entity.Country;
            set 
            { 
                SetProperty(Entity.Country, value, () => Entity.Country = value); 
            }
        }

        public string CameraCity
        {
            get => Entity.City;
            set { SetProperty(Entity.City, value, () => Entity.City = value); }
        }

        public string RtspAddress
        {
            get => Entity.RtspAddress;
            set { SetProperty(Entity.RtspAddress, value, () => Entity.RtspAddress = value); }
        }

        public double CameraLatitude
        {
            get => Entity.Latitude;
            set { SetProperty(Entity.Latitude, value, () => Entity.Latitude = value); }
        }

        public double CameraLongitude
        {
            get => Entity.Longitude;
            set { SetProperty(Entity.Longitude, value, () => Entity.Longitude = value); }
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
                await CamerasModel.GetLatitude(Entity);
            }

            if (!string.IsNullOrWhiteSpace(Longitude) && double.TryParse(Longitude, out longitude))
            {
                CameraLongitude = longitude;
            }
            else
            {
                await CamerasModel.GetLongitude(Entity);
            }

            model.AddCamera(Entity);
        }

        public bool IsValid()
        {
            return !(string.IsNullOrWhiteSpace(Entity.RtspAddress) || string.IsNullOrWhiteSpace(Entity.Country) ||
                string.IsNullOrWhiteSpace(Entity.City));
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
