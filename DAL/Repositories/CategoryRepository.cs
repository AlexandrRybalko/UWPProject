using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class CategoryRepository
    {
        private readonly DatabaseContext _ctx;

        public CategoryRepository()
        {
            _ctx = new DatabaseContext();
        }

        public IEnumerable<CategoryEntity> GetAll()
        {
            return _ctx.Categories.Include(x => x.CamerasCategories).ToList();
        }
    }
}
