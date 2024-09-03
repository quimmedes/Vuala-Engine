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

                //  await Task.Delay(3000);
                count = 30;
              

            
        }
    }

   




}
