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
    public class CameraViewModel : NotificationBase<Camera>
    {
		private ObservableCollection<Camera> _cameras = new ObservableCollection<Camera>();

		public ObservableCollection<Camera> Cameras
        {
            get
            {
				return this._cameras;
            }
        }

		public CameraViewModel()
		{
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
			_cameras.Add(new Camera { Country = "USA", City = "New-York" });
		}
	}
}
