using Microsoft.VisualBasic.FileIO;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EngineCSharp
{
    /// <summary>
    /// Represents a game object in the game world.
    /// </summary>
    public class GameObject : IDisposable
    {
        public Transform transform;
        public GameObject()
        {
            transform = new Transform();
            _mainThreadContext = SynchronizationContext.Current ?? new SynchronizationContext();

        }

        public Action component;
        private nint texturePointer;

        public nint texture
        {
            get { return texturePointer; }
            set
            {

                texturePointer = value;
                SDL.SDL_QueryTexture(texturePointer, out uint format, out int access, out transform.size.w, out transform.size.h);

            }
        }

        SynchronizationContext _mainThreadContext = SynchronizationContext.Current;


        /// <summary>
        /// Called when the game object is created.
        /// </summary>
        public virtual void Start()
        {
            _mainThreadContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Called every frame to update the game object.
        /// </summary>
        public virtual void Update()
        {
            component?.Invoke();
        }

        /// <summary>
        /// Called before rendering the game object.
        /// </summary>
        public virtual void BeforeRender()
        {

        }

        /// <summary>
        /// Called to render the game object.
        /// </summary>
        public virtual void Render()
        {
            if (texture != 0)
            {
                App.blit(texture, transform.position.x, transform.position.y);
            }
        }

        public void MainThread(Action action)
        {
           
            _mainThreadContext.Post(_ =>
            {
                action?.Invoke();
            }, null);
        }

        /// <summary>
        /// Called when a collision occurs with another game object.
        /// </summary>
        /// <param name="obj">The game object that collided with this game object.</param>
        /// <returns>True if a collision occurred, false otherwise.</returns>
        public virtual bool OnCollision(GameObject obj)
        {
            if (this != obj)
                return App.collision(transform.position.x, transform.position.y, transform.size.w, transform.size.h, obj.transform.position.x, obj.transform.position.y, obj.transform.size.w, obj.transform.size.h);

            return false;
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
                if (texture != IntPtr.Zero)
                {
                    SDL.SDL_DestroyTexture(texture);
                    texture = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Destroys the game object and removes it from the game.
        /// </summary>
        public void Destroy()
        {
            App.Remove(this);
            Dispose();
        }

        ~GameObject()
        {
            Dispose(false);

            if (texture != IntPtr.Zero)
            {
                SDL.SDL_DestroyTexture(texture);
                texture = IntPtr.Zero;
            }
        }
    }
    /// <summary>
    /// Represents the transform of a game object.
    /// </summary>
    public class Transform
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public Size size;

        public Transform()
        {
            position = new Vector3(0, 0, 0);
            rotation = new Vector3(0, 0, 0);
            scale = new Vector3(1, 1, 1);
        }
    }
    /// <summary>
    /// Represents the size of a game object.
    /// </summary>
    public struct Size
    {
        public int w;
        public int h;

        public Size(int w, int h)
        {
            this.w = w;
            this.h = h;
        }

        public override string ToString()
        {
            return $"({w}, {h})";
        }
    }

    /// <summary>
    /// Represents a 2D vector.
    /// </summary>
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(Vector2 a, float scalar)
        {
            return new Vector2(a.x * scalar, a.y * scalar);
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public Vector2 Normalize()
        {
            float magnitude = Magnitude();
            return new Vector2(x / magnitude, y / magnitude);
        }

        public static float Angle(Vector2 a, Vector2 b)
        {
            return (float)Math.Acos(Dot(a, b) / (a.Magnitude() * b.Magnitude()));
        }

        public static Vector2 Project(Vector2 a, Vector2 b)
        {
            return b * (Dot(a, b) / Dot(b, b));
        }

        public static Vector2 Reflect(Vector2 a, Vector2 normal)
        {
            return a - normal * 2 * Dot(a, normal);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return a * (1 - t) + b * t;
        }

        public static Vector2 Slerp(Vector2 a, Vector2 b, float t)
        {
            float dot = Dot(a, b);
            dot = Math.Clamp(dot, -1, 1);
            float theta = (float)Math.Acos(dot) * t;
            Vector2 relative = b - a * dot;
            relative = relative.Normalize();
            return (a * (float)Math.Cos(theta) + relative * (float)Math.Sin(theta));
        }
    }




    /// <summary>
    /// Represents a 3D vector.
    /// </summary>
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator *(Vector3 a, float scalar)
        {
            return new Vector3(a.x * scalar, a.y * scalar, a.z * scalar);
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3 Normalize()
        {
            float magnitude = Magnitude();
            return new Vector3(x / magnitude, y / magnitude, z / magnitude);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y * b.z - a.z * b.y,
                               a.z * b.x - a.x * b.z,
                               a.x * b.y - a.y * b.x);
        }

        public static float Angle(Vector3 a, Vector3 b)
        {
            return (float)Math.Acos(Dot(a, b) / (a.Magnitude() * b.Magnitude()));
        }

        public static Vector3 Project(Vector3 a, Vector3 b)
        {
            return b * (Dot(a, b) / Dot(b, b));
        }

        public static Vector3 Reflect(Vector3 a, Vector3 normal)
        {
            return a - normal * 2 * Dot(a, normal);
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return a * (1 - t) + b * t;
        }

        public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
        {
            float dot = Dot(a, b);
            dot = Math.Clamp(dot, -1, 1);
            float theta = (float)Math.Acos(dot) * t;
            Vector3 relative = b - a * dot;
            relative = relative.Normalize();
            return (a * (float)Math.Cos(theta) + relative * (float)Math.Sin(theta));
        }
    }

}
