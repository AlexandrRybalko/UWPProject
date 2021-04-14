using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class CategoryRepository : System.IDisposable
    {
        private readonly DatabaseContext ctx;

        public CategoryRepository()
        {
            ctx = new DatabaseContext();
        }

        public IEnumerable<CategoryEntity> GetAll()
        {
            return ctx.Categories.Include(x => x.CamerasCategories).ToList();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ctx.Dispose();
        }
    }
}
