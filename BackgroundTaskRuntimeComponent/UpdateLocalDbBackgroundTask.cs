using DAL.Entities;
using DAL.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Windows.ApplicationModel.Background;

namespace BackgroundTaskRuntimeComponent
{
    public sealed class UpdateLocalDbBackgroundTask : IBackgroundTask, IDisposable
    {
        private BackgroundTaskDeferral deferral;
        private CameraRepository cameraRepository = new CameraRepository();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance == null)
            {
                throw new ArgumentNullException(nameof(taskInstance));
            }

            deferral = taskInstance.GetDeferral();

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(new Uri("https://localhost:44389/api/Cameras")).ConfigureAwait(true);
            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(true);

            StreamReader streamReader = new StreamReader(contentStream);
            JsonTextReader jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();

            var cameras = serializer.Deserialize<List<CameraEntity>>(jsonReader);
            foreach (var camera in cameras)
            {
                camera.Id = 0;
            }

            cameraRepository.UpdateCameras(cameras);

            deferral.Complete();

            client.Dispose();
            streamReader.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                cameraRepository.Dispose();
            }
        }
    }
}
