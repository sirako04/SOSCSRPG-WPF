using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;

namespace Engine
{
    public  static class Sound
    {
        public  static Task Playing(string file)
        {
            SoundPlayer soundPlayer = new SoundPlayer();
            soundPlayer.SoundLocation = AppDomain.CurrentDomain.BaseDirectory +file;
            if (soundPlayer.SoundLocation != null && soundPlayer.IsLoadCompleted)
            {       
                soundPlayer.PlayLooping();
            }
            else
            {
                throw new ArgumentException($" {file} not found!");
            }
            return Task.CompletedTask;
        }
    }
}
