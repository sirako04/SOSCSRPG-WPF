using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;
using System.ComponentModel;
using System.Configuration.Assemblies;

namespace Engine.ViewModels
{
    public class GameSession : INotifyPropertyChanged
    {
        private Location _currentLocation;
        public World CurrentWorld { get; set; }
        public Player CurrentPlayer { get; set; }
        public Location CurrentLocation 
        { 
            get { return _currentLocation; }
            set 
            {
                _currentLocation = value; 
                OnPropertyChanged(nameof(CurrentLocation));
            }
        }
        public GameSession()
        {
            Player CurrentPlayer = new Player("Sirak", "Fighter", 15, 0, 1, 50);
            CurrentLocation = new Location(0, -1, "Home", "This is your home",
                "/Engine;component/Images/Locations/Home.png");
            WorldFactory factory = new WorldFactory();
            CurrentWorld = factory.CreateWorld();
            CurrentLocation = CurrentWorld.locationAt(0, -1);
        }
        public void MoveNorth()
        {
            CurrentLocation = CurrentWorld.locationAt
            (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
        }
        public void MoveEast()
        {
            CurrentLocation = CurrentWorld.locationAt
            (CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);

        }
        public void MoveSouth()
        {
            CurrentLocation = CurrentWorld.locationAt
            (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
        }
        public void MoveWest()
        {
            CurrentLocation = CurrentWorld.locationAt
            (CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
            {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
    }
}
