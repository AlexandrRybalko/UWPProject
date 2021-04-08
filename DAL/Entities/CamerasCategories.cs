using System;

namespace DAL.Entities
{
    public class CamerasCategories
    {
        public int CameraId { get; set; }
        public CameraEntity Camera { get; set; }

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
