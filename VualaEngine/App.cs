using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static EngineCSharp.Vuala.SDLCS.SDL;
using static EngineCSharp.Vuala.SDLCS.SDL_image;

namespace EngineCSharp.Vuala
{


    public class App
    {

        public static nint window = nint.Zero;
        public static nint render = nint.Zero;
        public const int Window_Width = 1280;
        public const int Window_Height = 720;
        const int SIDE_PLAYER = 0;
        const int SIDE_ENEMY = 1;
        const int FPS = 60;
        static ulong lastTime = 0;
        private static float deltaTime;
        public static List<GameObject> gameObjects = new List<GameObject>();
        static ConcurrentStack<GameObject> instantiates = new ConcurrentStack<GameObject>();
        static ConcurrentStack<GameObject> exclusions = new ConcurrentStack<GameObject>();
        //MultiThreadRelated
        public static AutoResetEvent _mainThreadEvent;
        public static SynchronizationContext _mainThreadContext;


        public static void Instantiate(GameObject gameObject)
        {
            Parallel.Invoke(() => instantiates.Push(gameObject));

        }

        public static float DeltaTime()
        {
            ulong currentTime = SDL_GetPerformanceCounter();
            ulong frequency = SDL_GetPerformanceFrequency();
            float deltaTime = (float)(currentTime - lastTime) / frequency;
            lastTime = currentTime;
            return deltaTime;
        }

        static void InstantiateAll()
        {
            if (instantiates.TryPop(out GameObject result))
            {
                if (result != null)
                {
                    gameObjects.Add(result);
                }
            }

        }

        public static void Remove(GameObject gameObject)
        {
            exclusions.Push(gameObject);

        }

        static void RemoveAll()
        {
            if (exclusions.TryPop(out GameObject result))
            {
                if (result != null)
                {
                    gameObjects.Remove(result);
                }
            }

        }

        public static nint LoadTextureFromMemory(nint renderer, byte[] imageData)
        {
            GCHandle handle = GCHandle.Alloc(imageData, GCHandleType.Pinned);
            nint imagePtr = handle.AddrOfPinnedObject();
            // Create SDL_RWops from byte array
            nint rwops = SDL_RWFromMem(imagePtr, imageData.Length);
            if (rwops == nint.Zero)
                throw new Exception("Failed to create RWops: " + SDL_GetError());

            // Load texture from RWops
            nint texture = IMG_LoadTexture_RW(renderer, rwops, 1);
            if (texture == nint.Zero)
                throw new Exception("Failed to load texture from memory: " + IMG_GetError());

            handle.Free();
            return texture;
        }



        public static void Main(string[] args)
        {

            Init();

            lastTime = SDL_GetPerformanceCounter();

            //MultiThread Related
            _mainThreadEvent = new AutoResetEvent(true);


            // Get the assembly containing the classes
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Find all types that inherit from BaseClass
            var childTypes = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(GameObject)) && !t.IsAbstract);

            // Iterate through the types and do something with them
            foreach (var type in childTypes)
            {
                // Create an instance of the child class
                GameObject? instance = (GameObject)Activator.CreateInstance(type);

                // Call the DoSomething method on the instance
                instance?.Start();
                if (instance != null)
                    gameObjects.Add(instance);

            }

            foreach (var game in gameObjects)
            {
                game?.BeforeRender();

            }

            long then = SDL_GetTicks();
            float remainder = 0;


            while (true)
            {

                prepareScene();

                /*
              for(int i = 0; i < gameObjects.Count; i++)
               {
                    for (int j = 0; j < gameObjects.Count; j++)
                    {
                        gameObjects[i]?.OnCollision(gameObjects[j]);
                          
                        
                    }
                }
                */

                foreach (var game in gameObjects)
                {
                    game?.Render();

                }

                foreach (var game in gameObjects)
                {
                    game?.Update();

                }


                presentScene();

                InstantiateAll();
                RemoveAll();

                _mainThreadEvent.Set();


                capFrameRate(ref then, ref remainder);

            }
            SDL_Quit();
        }

        static void capFrameRate(ref long then, ref float remainder)
        {
            long wait;
            long frameTime;

            wait = 16 + (long)remainder;

            remainder -= (int)remainder;

            frameTime = SDL_GetTicks() - then;

            wait -= frameTime;

            if (wait < 1)
            {
                wait = 1;
            }

            SDL_Delay((uint)wait);

            remainder += 0.667f;

            then = SDL_GetTicks();
        }

        static void prepareScene()
        {
            SDL_SetRenderDrawColor(render, 0, 255, 128, 0);
            SDL_RenderClear(render);
        }

        static void presentScene()
        {
            SDL_RenderPresent(render);
        }

        public static void blit(nint texutre, float x, float y)
        {
            SDL_Rect dest = new SDL_Rect();
            dest.x = (int)x;
            dest.y = (int)y;

            SDL_QueryTexture(texutre, out uint format, out int access, out dest.w, out dest.h);
            SDL_RenderCopy(render, texutre, nint.Zero, ref dest);

        }

        static void Init()
        {
            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine("SDL_Image Could not be initialized");
            }
            else
            {
                Console.WriteLine("SDL_INICIALIZADO");
            }

            if (IMG_Init(IMG_InitFlags.IMG_INIT_PNG | IMG_InitFlags.IMG_INIT_JPG | IMG_InitFlags.IMG_INIT_WEBP | IMG_InitFlags.IMG_INIT_TIF) < 0)
            {
                Console.WriteLine("Não incializou a imagem");
            }
            else
            {
                Console.WriteLine("Inicializou a imagem");
            }



            window = SDL_CreateWindow("Game from C#", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, Window_Width, Window_Height, SDL_WindowFlags.SDL_WINDOW_MAXIMIZED);
            SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "linear");

            render = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            if (render < 0)
            {
                Console.WriteLine("Render could not be created");
            }

            Audio.AudioInit();
        }

        public static void clipToScreen(Transform transform)
        {
            if (transform != null)
            {
                if (transform.position.x < 0)
                {
                    transform.position.x = 0;
                }

                if (transform.position.y < 0)
                {
                    transform.position.y = 0;
                }

                if (transform.position.x > Window_Width - transform.size.w)
                {
                    transform.position.x = Window_Width - transform.size.w;
                }

                if (transform.position.y > Window_Height - transform.size.h)
                {
                    transform.position.y = Window_Height - transform.size.h;
                }
            }
        }

        public static bool collision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {

            SDL_Rect sprite1 = new SDL_Rect
            {
                x = (int)x1,
                y = (int)y1, // Y position
                w = (int)w1, // Width of the sprite
                h = (int)h1  // Height of the sprite
            };

            SDL_Rect sprite2 = new SDL_Rect
            {
                x = (int)x2,
                y = (int)y2, // Y position
                w = (int)w2, // Width of the sprite
                h = (int)h2  // Height of the sprite
            };


            bool a = Math.Max(x1, x2) < Math.Min(x1 + w1, x2 + w2) && Math.Max(y1, y2) < Math.Min(y1 + h1, y2 + h2);
            return a;
            //  SDL_bool a = SDL_HasIntersection(ref sprite1, ref sprite2);

            // Console.WriteLine("colisao eh " + a);
            //  return a == SDL_bool.SDL_TRUE ? true : false;
        }
    }
}
