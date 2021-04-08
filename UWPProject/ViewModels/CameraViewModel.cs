using System;
using System.Linq;
using UWPProject.Models;

namespace UWPProject.ViewModels
{
    public class CameraViewModel : NotificationBase<Camera>
    {
        private readonly CamerasModel _model = new CamerasModel();

        public CameraViewModel(Camera camera = null) : base(camera) 
        {
            AddToFavouritesCommand = new ButtonCommand(new Action(AddToFavourites), CanExecuteAddToFavouritesCommand);
        }

        public ButtonCommand AddToFavouritesCommand { get; }

        public bool CanExecuteAddToFavouritesCommand()
        {
            return !_model.GetFavourites().Any(x => x.Id == this.This.Id);
        }

        public int Id
        {
            get { return This.Id; }
        }

        public string RtspAddress
        {
            get { return This.RtspAddress; }
            set { SetProperty(This.RtspAddress, value, () => This.RtspAddress = value); }
        }

        public string Country
        {
            get { return This.Country; }
            set { SetProperty(This.Country, value, () => This.Country = value); }
        }

        public string City
        {
            get { return This.City; }
            set { SetProperty(This.City, value, () => This.City = value); }
        }

        public double Latitude 
        { 
            get { return This.Latitude; } 
        }

        public double Longitude 
        {
            get { return This.Longitude; } 
        }

        public string ToStringProperty
        {
            get { return $"{this.Country}, {this.City}"; }
        }

        public void AddToFavourites()
        {
            _model.AddToCategory(This.Id, "Favourites");
        }

        public void AddToRecent()
        {
            _model.AddToCategory(This.Id, "Recent");
        }

        public void RemoveFromFavourites()
        {
            _model.RemoveFromCategory(This.Id, "Favourites");
        }
    }
}
