using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
