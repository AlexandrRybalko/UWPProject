using AutoMapper;
using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.Entities;
using Windows.Web.Http;

namespace UWPProject.Models
{
    public class CamerasModel
    {
        private readonly CameraRepository cameraRepository;
        private readonly CategoryRepository categoryRepository;
        private readonly CameraCategoryRepository cameraCategoryRepository;
        private readonly IMapper mapper;

        public CamerasModel()
        {
            cameraRepository = new CameraRepository();
            categoryRepository = new CategoryRepository();
            cameraCategoryRepository = new CameraCategoryRepository();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Camera, CameraEntity>();
                cfg.CreateMap<Camera, CameraEntity>().ReverseMap();

                cfg.CreateMap<Category, CategoryEntity>();
                cfg.CreateMap<Category, CategoryEntity>().ReverseMap();

                cfg.CreateMap<Entities.CamerasCategories, DAL.Entities.CamerasCategories>();
                cfg.CreateMap<Entities.CamerasCategories, DAL.Entities.CamerasCategories>().ReverseMap();
            });
            mapper = new Mapper(mapperConfig);
        }

        public IEnumerable<Camera> GetAllCameras()
        {
            var cameras = cameraRepository.GetAll();
            return mapper.Map<IEnumerable<Camera>>(cameras);
        }

        public IEnumerable<Camera> GetCamerasByCategory(string categoryName)
        {
            if (categoryName.Equals("Favourites"))
            {
                return this.GetFavourites();
            }
            if (categoryName.Equals("Recent"))
            {
                return this.GetRecent();
            }
            else
            {
                return this.GetRandom();
            }
        }

        public IEnumerable<Camera> GetRandom()
        {
            return this.GetAllCameras().ToList().Shuffle();
        }

        public IEnumerable<Camera> GetRecent()
        {
            var categoryId = categoryRepository.GetAll()
                .FirstOrDefault(z => z.Title.Equals("Recent")).Id;

            var recentCameras = cameraRepository.GetAll()
                .Where(x => x.CamerasCategories
                .Any(y => y.CategoryId == categoryId))
                .OrderByDescending(x => x.CamerasCategories
                .FirstOrDefault(y => y.CategoryId == categoryId).UpdatedTime);

            return mapper.Map<IEnumerable<Camera>>(recentCameras);
        }

        public IEnumerable<Camera> GetFavourites()
        {
            var favouriteCameras = cameraRepository.GetAll()
                .Where(x => x.CamerasCategories
                .Any(y => y.CategoryId == categoryRepository.GetAll()
                .FirstOrDefault(z => z.Title.Equals("Favourites")).Id));

            return mapper.Map<IEnumerable<Camera>>(favouriteCameras);
        }

        public Camera GetById(int id)
        {
            var camera = cameraRepository.GetById(id);
            return mapper.Map<Camera>(camera);
        }

        public void AddCamera(Camera camera)
        {
            var cameraEntity = mapper.Map<CameraEntity>(camera);
            cameraRepository.Add(cameraEntity);
        }

        public void AddToCategory(int cameraId, string categoryName)
        {
            cameraRepository.AddToCategory(cameraId, categoryName);
        }

        public void RemoveFromCategory(int cameraId, string categoryName)
        {
            int categoryId = categoryRepository.GetAll().FirstOrDefault(x => x.Title.Equals(categoryName)).Id;
            cameraCategoryRepository.RemoveFromCategory(cameraId, categoryId);
        }

        public async Task GetLatitude(Camera camera)
        {
            StringBuilder sb = new StringBuilder("https://api.ipgeolocation.io/ipgeo?apiKey=4def6b275e0b429d8f133f0f55ffd0ba&ip=");
            sb.Append(camera.RtspAddress.Split('/')[2]);
            Uri requestURI = new Uri(sb.ToString());

            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await client.GetAsync(requestURI);
            string responseString = await response.Content.ReadAsStringAsync();

            camera.Latitude = this.GetLatitude(responseString);
        }

        public async Task GetLongitude(Camera camera)
        {
            StringBuilder sb = new StringBuilder("https://api.ipgeolocation.io/ipgeo?apiKey=4def6b275e0b429d8f133f0f55ffd0ba&ip=");
            sb.Append(camera.RtspAddress.Split('/')[2]);
            Uri requestURI = new Uri(sb.ToString());

            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await client.GetAsync(requestURI);
            string responseString = await response.Content.ReadAsStringAsync();

            camera.Longitude = this.GetLongitude(responseString);
        }

        private double GetLatitude(string httpResponse)
        {
            double result = 0d;
            string latitude;
            var arr = httpResponse.Split(',');

            foreach (var str in arr)
            {
                if (str.Contains("latitude"))
                {
                    latitude = str.Split(':')[1].Remove(0, 1);
                    latitude = latitude.Remove(latitude.Length - 1);

                    result = Double.Parse(latitude);
                }
            }

            return result;
        }

        private double GetLongitude(string httpResponse)
        {
            double result = 0d;
            string longtitude;
            var arr = httpResponse.Split(',');

            foreach (var str in arr)
            {
                if (str.Contains("longitude"))
                {
                    longtitude = str.Split(':')[1].Remove(0, 1);
                    longtitude = longtitude.Remove(longtitude.Length - 1);

                    result = Double.Parse(longtitude);
                }
            }

            return result;
        }
    }
}
