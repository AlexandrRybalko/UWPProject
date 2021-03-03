using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPProject.Entities.DAL
{
    public class Camera
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
