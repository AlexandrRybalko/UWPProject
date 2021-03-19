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
        private readonly CameraRepository _repository;
        private readonly IMapper _mapper;

        public CamerasModel()
        {
            _repository = new CameraRepository();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Camera, CameraEntity>();
                cfg.CreateMap<Camera, CameraEntity>().ReverseMap();
            });
            _mapper = new Mapper(mapperConfig);
        }

        public IEnumerable<Camera> GetAll()
        {
            var cameras = _repository.GetAll();
            return _mapper.Map<IEnumerable<Camera>>(cameras);
        }

        public Camera GetById(int id)
        {
            var camera = _repository.GetById(id);
            return _mapper.Map<Camera>(camera);
        }

        public void AddCamera(Camera camera)
        {
            var cameraEntity = _mapper.Map<CameraEntity>(camera);
            _repository.Add(cameraEntity);
        }

        public void AddCameraToFavourite(Camera camera)
        {
            var cameraEntity = _mapper.Map<CameraEntity>(camera);
            //_repository.AddCameraToFavourite(cameraEntity);
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class Camera
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ImageType { get; set; }
    }
}
