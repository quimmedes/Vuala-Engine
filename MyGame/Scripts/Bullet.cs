﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineCSharp.Vuala;
using static EngineCSharp.Vuala.App;

namespace EngineCSharp.MyGame.Scripts
{


    /// <summary>
    /// Represents a bullet in the game.
    /// </summary>
    public class Bullet : GameObject
    {
        /// <summary>
        /// Updates the bullet's position and checks for collisions.
        /// </summary>
        public override void Update()
        {
            if (texture != 0)
            {
                transform.position.x += 8;

                if (transform.position.x > Window_Width)
                {
                    Destroy();
                    Console.WriteLine("Destroyed Bullet");
                }

                if (transform.position.y > Window_Height)
                {
                    Destroy();
                }

                foreach (var obj in gameObjects)
                {
                    OnCollision(obj);
                }
            }
        }

        /// <summary>
        /// Handles collision with other game objects.
        /// </summary>
        /// <param name="obj">The game object to check collision with.</param>
        /// <returns>True if collision occurred, false otherwise.</returns>
        public override bool OnCollision(GameObject obj)
        {
            if (base.OnCollision(obj))
            {
                if (obj is Enemy)
                {
                    Console.WriteLine("Collision occurred");
                    obj.Destroy();
                }

                return true;
            }

            return false;
        }
    }
}
