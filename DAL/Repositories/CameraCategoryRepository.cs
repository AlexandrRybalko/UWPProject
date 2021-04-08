using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class CameraCategoryRepository
    {
        private readonly DatabaseContext _ctx;

        public CameraCategoryRepository()
        {
            _ctx = new DatabaseContext();
        }

        public IEnumerable<CamerasCategories> GetAll()
        {
            return _ctx.CamerasCategories.ToList();
        }

        public void RemoveFromCategory(int cameraId, int categoryId)
        {
            var cameraCategory = _ctx.CamerasCategories.FirstOrDefault(x => x.CameraId == cameraId && x.CategoryId == categoryId);
            _ctx.CamerasCategories.Remove(cameraCategory);

            _ctx.SaveChanges();
        }
    }
}
