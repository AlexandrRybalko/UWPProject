using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CameraEntity
    {
        public int Id { get; set; }
        public string RtspAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<CamerasCategories> CamerasCategories { get; set; }
    }
}
