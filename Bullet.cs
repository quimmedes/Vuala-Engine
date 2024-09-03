using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngineCSharp.App;

namespace EngineCSharp
{


    public class Bullet : GameObject
    {

        public override void Update()
        {
            if (texture != 0)
            {
                transform.position.x += 8;

                if (transform.position.x > App.Window_Width)
                {
                    Destroy();
                    Console.WriteLine("Destrui Balas");
                }

                if (transform.position.y > App.Window_Height)
                {
                    Destroy();
                }



                for (int i = 0; i < gameObjects.Count; i++)
                {
                    OnCollision(gameObjects[i]);
                }
            }


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
