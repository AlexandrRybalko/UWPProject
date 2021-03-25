using AutoMapper;
using DAL;
using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPProject.Models
{
    public class CamerasModel
    {
        private readonly CameraRepository _cameraRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CamerasModel()
        {
            _cameraRepository = new CameraRepository();
            _categoryRepository = new CategoryRepository();

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
    }

    public class Camera
    {
        public int Id { get; set; }
        public string RtspAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

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
