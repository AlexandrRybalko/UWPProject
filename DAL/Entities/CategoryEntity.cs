using System.Collections.Generic;

namespace DAL.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<CamerasCategories> CamerasCategories { get; set; }
    }
}
