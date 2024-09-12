# Vuala C# Game Engine

Vuala Engine is engine inspired by Unity3d sintax Built upon SDL2 and with SDL2-CS bindings, the projet still early so there is no editor and only basic functionality implemented,
but feel free to do as you please and you can always use the SDL-cs bindings and implement more stuff yourself.


- All classes children of GameObject are automatically initialized
-To create a sprite showing on the screen, create a class and inherit from Gameobject, and Override the Start function with the texture and position, etc.
-To check collision override OnCollision or call OnCollision and pass as parameter the GameObject you want to compare it with, 
- All gameObjects are in App.gameObjects just loop trough the list and call oncolision(gameobject);


-Steps

- Create a class and Inherit from GameObject
- All classes children of GameObject are automatically initialized on runtime automatically;
- Override Start for setting initialization like texture, position and rotation;
- Overide Update for setting movement and collisions;
- Register your sprites in the Project->Properties->Resources before loading them

```C#
Public class Player : GameObject{

public override void Start()
{

    texture = App.LoadTextureFromMemory(render, Properties.Resources.player);
    transform.position.x = 100;
    transform.position.y = 100;
}

public override void Update() { }

}

   
public class Enemy : GameObject { }

public class Bullet : GameObject {
   
public override void Update()
{
    if (texture == 0)
        return;
         
    transform.position.x += 8;

    //Destroy Bullet if out of bounds of screen
             if (transform.position.x > App.Window_Width)
             {
                 Destroy();
                 Console.WriteLine("Destroyed Bullet");
             }

             //Destroy Bullet if out of bounds of screen
             if (transform.position.y > App.Window_Height)
             {
                 Destroy();
             }

            //Compare Global Gameobjects with OnCollision to see if it was hit, each frame;
            foreach(var obj in App.gameObjects)
             {
                 OnCollision(obj);
             }
         
}


public override bool OnCollision(GameObject obj){

         if (base.OnCollision(obj))
         {
            //if obj can convert to Enemy so it means is is an enemy so you can destroy it, 
            //it works like a Tag but uses the Type for comparison, as long your enemies inherit from the Type you want it to compare it works.
             if (obj is Enemy)
             {
                 Console.WriteLine("Collision occurred");
                 obj.Destroy();
                  return true;
             }
            
         }

         return false;
     }
}





```
