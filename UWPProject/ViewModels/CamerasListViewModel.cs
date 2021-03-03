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
		private ObservableCollection<CameraModel> _cameras = new ObservableCollection<CameraModel>();

		public ObservableCollection<CameraModel> Cameras
        {
            get
            {
				return this._cameras;
            }
        }


		public CamerasListViewModel()
		{
			_cameras.Add(new CameraModel { Id = 1, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Id = 2, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Id = 3, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Id = 4, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Id = 5, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Id = 6, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Id = 7, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Id = 8, Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
			_cameras.Add(new CameraModel { Country = "USA", City = "New-York" });
		}
	}
}
