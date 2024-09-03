using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace EngineCSharp
{
   
    
    public class Engine
    {
        public const uint SDL_INIT_TIMER = 0x00000001u;
        public const uint SDL_INIT_AUDIO = 0x00000010u;
        public const uint SDL_INIT_VIDEO = 0x00000020u; /**< SDL_INIT_VIDEO implies SDL_INIT_EVENTS */
        public const uint SDL_INIT_JOYSTICK = 0x00000200u;  /**< SDL_INIT_JOYSTICK implies SDL_INIT_EVENTS */
        public const uint SDL_INIT_HAPTIC = 0x00001000u;
        public const uint SDL_INIT_GAMECONTROLLER = 0x00002000u;  /**< SDL_INIT_GAMECONTROLLER implies SDL_INIT_JOYSTICK */
        public const uint SDL_INIT_EVENTS = 0x00004000u;
        public const uint SDL_INIT_SENSOR = 0x00008000u;
        public const uint SDL_INIT_NOPARACHUTE = 0x00100000u;  /**< compatibility; this flag is ignored. */
        public const uint SDL_INIT_EVERYTHING = ( SDL_INIT_TIMER | SDL_INIT_AUDIO | SDL_INIT_VIDEO | SDL_INIT_EVENTS | 
                SDL_INIT_JOYSTICK | SDL_INIT_HAPTIC | SDL_INIT_GAMECONTROLLER | SDL_INIT_SENSOR );

        //Renderer flags

        public const int SDL_RENDERER_SOFTWARE = 0x00000001;         /**< The renderer is a software fallback */
        public const int SDL_RENDERER_ACCELERATED = 0x00000002;      /**< The renderer uses hardware
                                                     acceleration */
        public const int SDL_RENDERER_PRESENTVSYNC = 0x00000004;     /**< Present is synchronized
                                                     with the refresh rate */
        public const int SDL_RENDERER_TARGETTEXTURE = 0x00000008;     /**< The renderer supports
                                                     rendering to texture */


        private const string nativeLibName = "SDL2";

        [DllImport(nativeLibName, CallingConvention = CallingConvention.StdCall)]
        public static extern int SDL_Init(uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.StdCall)]
        public static extern void SDL_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SDL_CreateRenderer(IntPtr window, int index, uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.StdCall)]
        
        public static extern IntPtr SDL_CreateWindow([MarshalAs(UnmanagedType.LPStr)] string title, int x, int y, int w, int h, uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.StdCall)]
        public static extern void SDL_Delay(uint ms);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.StdCall)]
        public static extern int SDL_SetHint([MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string hint);


    }
}
