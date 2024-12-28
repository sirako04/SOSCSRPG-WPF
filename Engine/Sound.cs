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
        public static Task Playing(string file)
        {
            bool isPlaying = false;

            SoundPlayer soundPlayer = new SoundPlayer
            {
                SoundLocation = file
            };

            if (isPlaying)
            {
                return null; 
            }

            if (soundPlayer.SoundLocation != null)
            {       
                soundPlayer.PlayLooping();
            }
            else
            {
                throw new ArgumentException($" {file} not found!");
            }
            
            return Task.CompletedTask;
        }
        public static void Stop(string file) 
        {
            SoundPlayer soundPlayer = new SoundPlayer();
            soundPlayer.SoundLocation = file;
            
                soundPlayer.Stop();
                return;            
        }
    }
}
