using AutoMapper;
using DAL;
using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace UWPProject.Models
{
    public class CamerasModel
    {
        private readonly CameraRepository _cameraRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly CameraCategoryRepository _cameraCategoryRepository;
        private readonly IMapper _mapper;

        public CamerasModel()
        {
            _cameraRepository = new CameraRepository();
            _categoryRepository = new CategoryRepository();
            _cameraCategoryRepository = new CameraCategoryRepository();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Camera, CameraEntity>();
                cfg.CreateMap<Camera, CameraEntity>().ReverseMap();

                cfg.CreateMap<Category, CategoryEntity>();
                cfg.CreateMap<Category, CategoryEntity>().ReverseMap();

                cfg.CreateMap<CamerasCategories, DAL.Entities.CamerasCategories>();
                cfg.CreateMap<CamerasCategories, DAL.Entities.CamerasCategories>().ReverseMap();
            });
            _mapper = new Mapper(mapperConfig);
        }

        public IEnumerable<Camera> GetAllCameras()
        {
            var cameras = _cameraRepository.GetAll();
            return _mapper.Map<IEnumerable<Camera>>(cameras);
        }

        public IEnumerable<Camera> GetCamerasByCategory(string categoryName)
        {
            if (categoryName.Equals("Favourites"))
            {
                return this.GetFavourites();
            }
            else if (categoryName.Equals("Recent"))
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
            var categoryId = _categoryRepository.GetAll()
                .FirstOrDefault(z => z.Title.Equals("Recent")).Id;

            var recentCameras = _cameraRepository.GetAll()
                .Where(x => x.CamerasCategories
                .Any(y => y.CategoryId == categoryId))
                .OrderByDescending(x => x.CamerasCategories
                .FirstOrDefault(y => y.CategoryId == categoryId).UpdatedTime);

            return _mapper.Map<IEnumerable<Camera>>(recentCameras);
        }

        public IEnumerable<Camera> GetFavourites()
        {
            var favouriteCameras = _cameraRepository.GetAll()
                .Where(x => x.CamerasCategories
                .Any(y => y.CategoryId == _categoryRepository.GetAll()
                .FirstOrDefault(z => z.Title.Equals("Favourites")).Id));

            return _mapper.Map<IEnumerable<Camera>>(favouriteCameras);
        }

        public Camera GetById(int id)
        {
            var camera = _cameraRepository.GetById(id);
            return _mapper.Map<Camera>(camera);
        }

        public void AddCamera(Camera camera)
        {
            var cameraEntity = _mapper.Map<CameraEntity>(camera);
            _cameraRepository.Add(cameraEntity);
        }

        public void AddToCategory(int cameraId, string categoryName)
        {
            _cameraRepository.AddToCategory(cameraId, categoryName);
        }

        public void RemoveFromCategory(int cameraId, string categoryName)
        {
            int categoryId = _categoryRepository.GetAll().FirstOrDefault(x => x.Title.Equals(categoryName)).Id;
            _cameraCategoryRepository.RemoveFromCategory(cameraId, categoryId);
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

    public class Camera
    {
        public int Id { get; set; }
        public string RtspAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<CamerasCategories> CamerasCategories { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<CamerasCategories> CamerasCategories { get; set; }
    }

    public class CamerasCategories
    {
        public int CameraId { get; set; }
        public int CategoryId { get; set; }
    }
}
