using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.DAL;

namespace UWPProject.Repositories.DAL
{
    public class CameraRepository
    {
        private readonly ApplicationContext _ctx;

        public CameraRepository()
        {
            _ctx = new ApplicationContext();
        }


    }
}
