using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class CameraRepository
    {
        private readonly DatabaseContext _ctx;

        public CameraRepository()
        {
            _ctx = new DatabaseContext();
        }

        public IEnumerable<CameraEntity> GetAll()
        {
            return _ctx.Cameras.ToList();
        }

        public void Add(CameraEntity camera)
        {
            _ctx.Cameras.Add(camera);
            _ctx.SaveChanges();
        }

        public CameraEntity GetById(int id)
        {
            return _ctx.Cameras.FirstOrDefault(x => x.Id == id);
        }
    }
}
