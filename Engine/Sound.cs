using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;

namespace Engine
{
    public static class Sound
    {
        private static bool isPlaying = false;
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
        public static Task PlayingMp3(string file)
        {

            var Mp3FileReader = new Mp3FileReader(file);
            var waveOut = new WaveOut();
            if (!isPlaying)
            {
                waveOut.Init(Mp3FileReader);
                waveOut.Play();
                isPlaying = true;
            }

            return Task.CompletedTask;
        }
    }
}
