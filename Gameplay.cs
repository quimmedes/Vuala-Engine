using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static SDL2.SDL;
using static SDL2.SDL_image;
using static EngineCSharp.App;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.Threading;

namespace EngineCSharp
{

    public class GameManager : GameObject
    {
        int count = 30;


        public override void Start()
        {
            Audio.AddAudio(Audio.SND.SND_PLAYER_FIRE, "sound/334227__jradcoolness__laser.ogg");
            Audio.AddAudio(Audio.SND.SND_ALIEN_FIRE, "sound/196914__dpoggioli__laser-gun.ogg");
            Audio.AddAudio(Audio.SND.SND_PLAYER_DIE, "sound/245372__quaker540__hq-explosion.ogg");
            Audio.AddAudio(Audio.SND.SND_ALIEN_DIE, "sound/10 Guage Shotgun-SoundBible.com-74120584.ogg");

        }


        public override void Update()
        {
            SpawnEnemies();
        }


        void SpawnEnemies()
        {
            if (count-- > 0)
                return;

                Enemy enemy = new Enemy();
            
                
                enemy.texture = LoadTextureFromMemory(render, Properties.Resources.enemy);
                enemy.transform.position.x = Window_Width;
                enemy.transform.position.y = new Random().Next(Window_Height);
                Instantiate(enemy);

                count = 0;
         

        }
    }

   




}
