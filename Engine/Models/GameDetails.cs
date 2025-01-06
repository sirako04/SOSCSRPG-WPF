using System;
using System.Collections.Generic;

namespace Engine.Models
{
    public class GameDetails
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Version { get; set; }
        public List<Race> Races { get; } =
            new List<Race>();
        public List<PlayerAttribute> PlayerAttributes { get; set; } = new List<PlayerAttribute>();
        public GameDetails(string title,string subTitle, string version)
        {
            Title = title;
            SubTitle = subTitle;                                                                          
            Version = version;
        }
    }
}
