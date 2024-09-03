using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_image;


namespace EngineCSharp
{
    public class Enemy : GameObject
    {

        public override void Start()
        {
            texture = IMG_LoadTexture(App.render, "enemy.png");
            transform.position.x = 500;
            transform.position.y = 500;
        }

        public override void Update()
        {
            transform.position.x -= 2;
            //   transform.position.y += 2;

            if (transform.position.x < -transform.size.w)
            {
                Destroy();
                Console.WriteLine("Destruiu");
            }

            
        }



    }
}
