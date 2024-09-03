using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using static SDL2.SDL_image;
using static EngineCSharp.App;
using System.ComponentModel;
using System.Runtime.Versioning;
using SDL2;
using System.Runtime.InteropServices;

namespace EngineCSharp
{

    /// <summary>
    /// Represents a player object in the game.
    /// </summary>
    public class Player : GameObject
    {
        float PlayerSpeed = 4f;

        public static Player player;
        public int bulletCount = 8;

        /// <summary>
        /// Initializes the player object.
        /// </summary>
        /// 

        
        public override void Start()
        {

            texture = App.LoadTextureFromMemory(render, Properties.Resources.player);
            transform.position.x = 100;
            transform.position.y = 100;

            player = this;
        
        }

        /// <summary>
        /// Creates a bullet object and shoots it.
        /// </summary>
        private void makeBullet()
        {
            if (bulletCount-- >= 0)
                return;

            Bullet bullet = new Bullet();

            // byte[] imageData = ResourceHelper.GetEmbeddedResource("EngineCSharp.player.png");
            bullet.texture = App.LoadTextureFromMemory(render, Properties.Resources.bullet);
            bullet.transform.position.x = Player.player.transform.position.x;
            bullet.transform.position.y = Player.player.transform.position.y;
            Instantiate(bullet);
            bulletCount = 8;
        }

        /// <summary>
        /// Updates the player's position based on user input.
        /// </summary>
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
            }

            transform.position.x += dx;
            transform.position.y += dy;

        }

        /// <summary>
        /// Handles collision with other game objects.
        /// </summary>
        /// <param name="obj">The game object collided with.</param>
        /// <returns>True if collision was handled, false otherwise.</returns>
        public override bool OnCollision(GameObject obj)
        {
            if (base.OnCollision(obj))
            {
                if (obj is Enemy)
                {
                    Console.WriteLine("Colidiu");
                    obj.Destroy();
                    return true;

                }

            }

            return false;
        }
    }
}
