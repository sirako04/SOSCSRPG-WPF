using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.IO;

namespace Engine
{
    public  static class Sound
    {
        static bool isPlaying = false;
        public static Task Playing(string file)
        {
           
            SoundPlayer soundPlayer = new SoundPlayer
            {
                SoundLocation = file
            };
            if (!isPlaying)
            {

                if (soundPlayer.SoundLocation != null)
                {
                    soundPlayer.PlayLooping();
                    isPlaying = true;
                }
                else
                {
                    throw new FileNotFoundException($" {file} not found!");
                }
            }
            else
            {
                return Task.CompletedTask;
            }
            
            return Task.CompletedTask;
        }
        public static void Stop(string file) 
        {
            SoundPlayer soundPlayer = new SoundPlayer
            {
                SoundLocation = file
            };

            soundPlayer.Stop();
            isPlaying = false;
                return;            
        }
    }
}
