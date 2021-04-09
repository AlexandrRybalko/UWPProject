using DAL;
using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace UpdateLocalDbBackgroundTask
{
    public sealed class UpdateLocalDbBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private CamerasDbContext _ctx = new CamerasDbContext();
        private CameraRepository _cameraRepository = new CameraRepository();

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var newCameras =_ctx.GetNewCameras();
            List<CameraEntity> cameras = new List<CameraEntity>();

            foreach (var camera in newCameras)
            {
                cameras.Add(new CameraEntity
                {
                    RtspAddress = camera.RtspAddress,
                    Country = camera.Country,
                    City = camera.City,
                    Latitude = camera.Latitude,
                    Longitude = camera.Longitude
                });
            }

            _cameraRepository.UpdateCameras(cameras);
            _deferral.Complete();
        }
    }
}
