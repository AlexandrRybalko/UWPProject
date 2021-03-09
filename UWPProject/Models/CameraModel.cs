using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UWPProject.Models
{
    public class CameraModel : INotifyPropertyChanged
    {
        private int _id;
        private string _ipAddress;
        private string _country;
        private string _city;
        public int Id { get; set; }
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                this.OnPropertyChanged();
            }
        }
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                this.OnPropertyChanged();
            }
        }
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
