using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_image;
using static EngineCSharp.App;

namespace EngineCSharp
{

    public class Player : GameObject
    {
        float PlayerSpeed = 4f;

        public static Player player;
        public int bulletCount = 8;

        public override void Start()
        {
            texture = IMG_LoadTexture(App.render, "player.png");
            transform.position.x = 100;
            transform.position.y = 100;





            player = this;

        }

        void makeBullet()
        {
            if (bulletCount-- >= 0)
                return;

            Bullet bullet = new Bullet();
            bullet.texture = IMG_LoadTexture(App.render, "bullet.png");
            bullet.transform.position.x = Player.player.transform.position.x;
            bullet.transform.position.y = Player.player.transform.position.y;
            Instantiate(bullet);
            bulletCount = 8;

        }

        public override void Update()
        {
            float dx = 0;
            float dy = 0;

            if (Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_W] == 1 || Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_UP] == 1)
            {
                dy = -PlayerSpeed;
            }

            if (Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_S] == 1 || Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_DOWN] == 1)
            {
                dy = PlayerSpeed;
            }

            if (Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_A] == 1 || Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_LEFT] == 1)
            {
                dx = -PlayerSpeed;
            }

            if (Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_D] == 1 || Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_RIGHT] == 1)
            {
                dx = PlayerSpeed;
            }

            if (Input.keyboard[(int)SDL_Scancode.SDL_SCANCODE_SPACE] == 1)
            {

                makeBullet();
                //shoot
                //Console.WriteLine("Shot");
                // fireBullet(player);

            }



            //Diagonal movement
            transform.position.x += dx;
            transform.position.y += dy;

            //Collision Checker;




        }

        public override bool OnCollision(GameObject obj)
        {
            if (base.OnCollision(obj))
            {

                if (obj is Enemy)
                {
                    Console.WriteLine("Colidiu");
                    obj.Destroy();

                }

                return true;

            }



            return false;
        }


    }
}
