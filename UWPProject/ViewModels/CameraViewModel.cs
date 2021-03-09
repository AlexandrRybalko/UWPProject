using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPProject.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DAL;
using AutoMapper;

namespace UWPProject.ViewModels
{
    public class CameraViewModel
    {
        private Repository _repository = new Repository();
        private IMapper _mapper;
        private CameraModel _cameraModel;

        public CameraViewModel(int id)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Camera, CameraModel>();
                cfg.CreateMap<Camera, CameraModel>().ReverseMap();
            });
            _mapper = new Mapper(mapperConfig);

            var camera = _repository.GetCamera(id);
            _cameraModel = _mapper.Map<CameraModel>(camera);
        }

        public int Id
        {
            get => _cameraModel.Id;
        }

        public string City
        {
            get => _cameraModel.City;
        }

        public string Country
        {
            get => _cameraModel.Country;
        }

        public string ToStringProperty
        {
            get
            {
                return $"{_cameraModel.City}, {_cameraModel.Country}";
            }
        }
    }
}
