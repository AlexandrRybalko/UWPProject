using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class CameraCategoryRepository : System.IDisposable
    {
        private readonly DatabaseContext ctx;

        public CameraCategoryRepository()
        {
            ctx = new DatabaseContext();
        }

        public IEnumerable<CamerasCategories> GetAll()
        {
            return ctx.CamerasCategories.ToList();
        }

        public void RemoveFromCategory(int cameraId, int categoryId)
        {
            var cameraCategory = ctx.CamerasCategories.FirstOrDefault(x => x.CameraId == cameraId && x.CategoryId == categoryId);
            ctx.CamerasCategories.Remove(cameraCategory);

            ctx.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ctx.Dispose();
        } 
    }
}
