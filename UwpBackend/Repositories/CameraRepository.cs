using System.Collections.Generic;
using System.Linq;
using UwpBackend.Interfaces;
using UwpBackend.Models;

namespace UwpBackend.Repositories
{
    public class CameraRepository : ICameraRepository
    {
        private readonly CamerasDbContext ctx;

        public CameraRepository(CamerasDbContext context)
        {
            ctx = context;
        }

        public IEnumerable<Camera> GetAll()
        {
            return ctx.Cameras.ToList();
        }

        public Camera GetById(int id)
        {
            return ctx.Cameras.FirstOrDefault(x => x.Id == id);
        }
    }
}
