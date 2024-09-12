using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static EngineCSharp.Vuala.SDLCS.SDL;
using static EngineCSharp.Vuala.SDLCS.SDL_image;
using static EngineCSharp.Vuala.App;
using EngineCSharp.Vuala;

namespace EngineCSharp.MyGame.Scripts
{
    /// <summary>
    /// Represents an enemy object in the game.
    /// </summary>
    public class Enemy : GameObject
    {

        /// <summary>
        /// Initializes the enemy object.
        /// </summary>
        public override void Start()
        {
            texture = LoadTextureFromMemory(render, Properties.Resources.enemy);
            transform.position.x = 500;
            transform.position.y = 500;
        }

        /// <summary>
        /// Updates the enemy object.
        /// </summary>
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
