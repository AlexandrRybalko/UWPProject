using System.Collections.Generic;
using UwpBackend.Models;

namespace UwpBackend.Interfaces
{
    public interface ICameraRepository
    {
        IEnumerable<Camera> GetAll();
        Camera GetById(int id);
    }
}
