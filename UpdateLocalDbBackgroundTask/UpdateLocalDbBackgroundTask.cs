using DAL;
using DAL.Entities;
using DAL.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace UpdateLocalDbBackgroundTask
{
    public sealed class UpdateLocalDbBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral deferral;
        private CameraRepository cameraRepository = new CameraRepository();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(new Uri("https://localhost:44389/api/Cameras"));
            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync();

            var streamReader = new StreamReader(contentStream);
            var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();

            try
            {
                var cameras = serializer.Deserialize<List<CameraEntity>>(jsonReader);
                foreach (var camera in cameras)
                {
                    camera.Id = 0;
                }

                cameraRepository.UpdateCameras(cameras);
            }
            catch (JsonReaderException)
            {
                Console.WriteLine("Invalid JSON.");
            }

            deferral.Complete();
        }
    }
}
