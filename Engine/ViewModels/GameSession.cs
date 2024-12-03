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
        public Player CurrentPlayer { get; set; }
        public GameSession() 
        {
            Player CurrentPlayer = new Player("Sirak","Fighter",15,0,1,50);
        }
    }
}
