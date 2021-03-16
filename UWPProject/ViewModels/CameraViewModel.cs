using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class CameraViewModel : NotificationBase<Camera>
    {
        public CameraViewModel(Camera camera = null) : base(camera) { }

        public CameraViewModel(int id)
        {
            var model = new CamerasModel();
            var camera = model.GetById(id);
        }

        public int Id
        {
            get { return This.Id; }
        }

        public string IpAddress
        {
            get { return This.IpAddress; }
            set { SetProperty(This.IpAddress, value, () => This.IpAddress = value); }
        }

        public string Country
        {
            get { return This.Country; }
            set { SetProperty(This.Country, value, () => This.Country = value); }
        }

        public string City
        {
            get { return This.City; }
            set { SetProperty(This.City, value, () => This.City = value); }
        }
        
        public string ImageType
        {
            get { return This.ImageType; }
            set { SetProperty(This.ImageType, value, () => This.ImageType = value); }
        }

        public string ToStringProperty
        {
            get { return $"{this.Country}, {this.City}"; }
        }
    }
}
