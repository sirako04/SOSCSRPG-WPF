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
    public class GameSession : Notification
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

                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(HasLocationToWest));
                GivePlayerQuestsAtLocation();
            }
        }
        public bool HasLocationToNorth
        {
            get
            {
                return CurrentWorld.locationAt
            (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
            }
        }
        public bool HasLocationToEast
        {
            get
            {
                return CurrentWorld.locationAt
            (CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
            }
        }
        public bool HasLocationToSouth
        {
            get
            {
                return CurrentWorld.locationAt
            (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
            }
        }
        public bool HasLocationToWest
        {
            get
            {
                return CurrentWorld.locationAt
            (CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
            }
        }
        public GameSession()
        {
            CurrentPlayer = new Player("Sirak", "Fighter", 15, 0, 1, 50);

            CurrentLocation = new Location(0, -1, "Home", "This is your home",
                "/Engine;component/Images/Locations/Home.png");
 
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.locationAt(0, -1);
            CurrentPlayer.Inventory.Add(ItemFactory.CreateGameItem(1001));
        }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.locationAt
                (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.locationAt
                (CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.locationAt
                (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.locationAt
                (CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }
        private void GivePlayerQuestsAtLocation() 
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q =>q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                }
            }
        }

    }
}
