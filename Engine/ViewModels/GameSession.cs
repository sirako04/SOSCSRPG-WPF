using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ViewModels
{
    public class GameSession
    {
        Player CurrentPlayer { get; set; }
        public GameSession() 
        {
            Player CurrentPlayer = new Player();
            CurrentPlayer.Name = "Sirak";
        }
    }
}
