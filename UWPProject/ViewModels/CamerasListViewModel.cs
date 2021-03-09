using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class CamerasListViewModel
    {
		private Repository _repository = new Repository();
		private ObservableCollection<CameraModel> _cameras = new ObservableCollection<CameraModel>();
		private IMapper _mapper;

		public ObservableCollection<CameraModel> Cameras
        {
            get
            {
				return this._cameras;
            }
        }


		public CamerasListViewModel()
		{
			var mapperConfig = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Camera, CameraModel>();
				cfg.CreateMap<Camera, CameraModel>().ReverseMap();
			});
			_mapper = new Mapper(mapperConfig);

			var cameras = _repository.GetCameras();
			_cameras = _mapper.Map<ObservableCollection<CameraModel>>(cameras);
		}
	}
}
