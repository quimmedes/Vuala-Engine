using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngineCSharp.Vuala.SDLCS.SDL;

namespace EngineCSharp.Vuala
{
    public class Input : GameObject
    {
        const int MAX_KEYBOARD_KEYS = 350;
        public static int[] keyboard = new int[MAX_KEYBOARD_KEYS];


        static void doKeyUp(ref SDL_KeyboardEvent e)
        {
            if (e.repeat == 0 && (int)e.keysym.scancode < MAX_KEYBOARD_KEYS)
            {
                keyboard[(int)e.keysym.scancode] = 0;

            }

        }

        static void doKeyDown(ref SDL_KeyboardEvent e)
        {
            if (e.repeat == 0 && (int)e.keysym.scancode < MAX_KEYBOARD_KEYS)
            {
                keyboard[(int)e.keysym.scancode] = 1;

            }

        }

        public override void Update()
        {
            doInput();
        }

        static void doInput()
        {

            while (SDL_PollEvent(out SDL_Event e) > 0)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        SDL_Quit();
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        doKeyDown(ref e.key);
                        break;
                    case SDL_EventType.SDL_KEYUP:
                        doKeyUp(ref e.key);
                        break;


                    default:
                        break;
                }

            }

        }

    }
}
