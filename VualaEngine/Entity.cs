using System;
using EngineCSharp;
using EngineCSharp.Vuala.SDLCS;

public class Entity : IDisposable
{
    public int x, y, w, h, dx, dy;
    public int health, reload;
    public int side;
    public nint texture;
    public Entity next;

    public Entity()
    {
        x = 0;
        y = 0;
        texture = IntPtr.Zero;
        health = 1;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Dispose managed resources here
            // For example, if texture is a managed resource, you can dispose it like this:
            if (texture != IntPtr.Zero)
            {
                SDL.SDL_DestroyTexture(texture);
                texture = IntPtr.Zero;
            }
        }

        // Dispose unmanaged resources here
        // There are no unmanaged resources in this class, so no additional cleanup is needed
    }

    ~Entity()
    {
        Dispose(false);
    }
}
