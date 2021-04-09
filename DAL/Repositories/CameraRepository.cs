using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _ctx.Cameras.Include(x => x.CamerasCategories).ToList();
        }

        public void Add(CameraEntity camera)
        {
            _ctx.Cameras.Add(camera);

            _ctx.SaveChanges();
        }

        public void AddToCategory(int cameraId, string categoryName)
        {
            var entity = new CamerasCategories
            {
                CameraId = cameraId,
                CategoryId = _ctx.Categories.FirstOrDefault(x => x.Title.Equals(categoryName)).Id,
                UpdatedTime = DateTime.UtcNow
            };

            var entityToUpdate = _ctx.CamerasCategories.FirstOrDefault(x => x.CameraId == entity.CameraId && 
            x.CategoryId == entity.CategoryId);

            if (entityToUpdate != null)
            {
                entityToUpdate.CameraId = entity.CameraId;
                entityToUpdate.CategoryId = entity.CategoryId;
                entityToUpdate.UpdatedTime = entity.UpdatedTime;
                _ctx.CamerasCategories.Update(entityToUpdate);
            }
            else
            {
                _ctx.CamerasCategories.Add(entity);
            }
            
            _ctx.SaveChanges();
        }

        public CameraEntity GetById(int id)
        {
            return _ctx.Cameras.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateCameras(IEnumerable<CameraEntity> newCameras)
        {
            var allCameras = this.GetAll().ToList();
            List<CameraEntity> camerasToDelete = new List<CameraEntity>();
            List<CameraEntity> camerasToAdd = new List<CameraEntity>();

            foreach (var cameraToDelete in allCameras)
            {
                if (!newCameras.Contains(cameraToDelete, new CameraEqualityComparer()))
                {
                    camerasToDelete.Add(cameraToDelete);
                }
            }

            foreach (var cameraToAdd in newCameras)
            {
                if (!allCameras.Contains(cameraToAdd, new CameraEqualityComparer()))
                {
                    camerasToAdd.Add(cameraToAdd);
                }
            }

            _ctx.Cameras.RemoveRange(camerasToDelete);
            _ctx.Cameras.AddRange(camerasToAdd);

            _ctx.SaveChanges();
        }
    }

    class CameraEqualityComparer : IEqualityComparer<CameraEntity>
    {
        public bool Equals(CameraEntity x, CameraEntity y)
        {
            if (x.RtspAddress.Equals(y.RtspAddress))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(CameraEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
