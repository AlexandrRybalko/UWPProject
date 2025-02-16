﻿using System;
using System.Threading.Tasks;
using UWPProject.Entities;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class NewCameraViewModel : NotificationBase<Camera>, System.IDisposable
    {
        private readonly CamerasModel model;

        public NewCameraViewModel()
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

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Entity.RtspAddress) || string.IsNullOrWhiteSpace(Entity.Country) 
                || string.IsNullOrWhiteSpace(Entity.City))
            {
                return false;
            }

            if (!Entity.RtspAddress.StartsWith("rtsp://", System.StringComparison.Ordinal) || Entity.RtspAddress.Length < 15)
            {
                return false;
            }

            return true;
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
