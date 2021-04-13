using System.Collections.Generic;

namespace UWPProject.Entities
{
    public class Camera
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
