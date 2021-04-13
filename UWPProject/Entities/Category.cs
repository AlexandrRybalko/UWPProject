using System.Collections.Generic;

namespace UWPProject.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<CamerasCategories> CamerasCategories { get; set; }
    }
}
