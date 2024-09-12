using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngineCSharp.Vuala.SDLCS.SDL_mixer;

namespace EngineCSharp.Vuala
{
    public class Audio
    {
        public static Dictionary<SND, nint> Audios = new Dictionary<SND, nint>();
        const int MAX_SND_CHANNELS = 8;
        private static nint music;
        public enum CH
        {
            CH_ANY = -1,
            CH_PLAYER,
            CH_ALIEN_FIRE
        };

        public enum SND
        {
            SND_PLAYER_FIRE,
            SND_ALIEN_FIRE,
            SND_PLAYER_DIE,
            SND_ALIEN_DIE,
            SND_MAX
        };

        public static void AudioInit()
        {
            if (Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 1024) == -1)
            {

                Console.WriteLine("Couldn't initlize SDL Mixer");

            }

            Mix_AllocateChannels(MAX_SND_CHANNELS);

        }

        public static void AddAudio(SND snd, string audio)
        {
            Audios[snd] = Mix_LoadWAV(audio);
        }

        public static void LoadMusic(string filename)
        {
            if (music != 0)
            {
                Mix_HaltMusic();
                Mix_FreeMusic(music);
                music = 0;
            }

            music = Mix_LoadMUS(filename);
        }

        public static void PlayMusic(int loop)
        {
            Mix_PlayMusic(music, loop == 0 ? 0 : -1);
        }

        public static void PlaySound(SND snd, int channel)
        {
            Mix_PlayChannel(channel, Audios[snd], 0);
        }

    }
}
