using Microsoft.VisualBasic.FileIO;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCSharp
{
    public class GameObject : IDisposable
    {
        public Transform transform;
        public GameObject() { 
            transform = new Transform();
        }

        public Action component;
        private nint texturePointer;

        public nint texture
        {
            get { return texturePointer; }
            set {

                texturePointer = value;
                SDL.SDL_QueryTexture(texturePointer, out uint format, out int access, out transform.size.w, out transform.size.h);

            }
        }


        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            component?.Invoke();
         //   Console.WriteLine("hello form fahter");
        }

        public virtual void BeforeRender()
        {
           

          


        }

        public virtual void Render()
        {
            if (texture != 0)
            {
                App.blit(texture, transform.position.x, transform.position.y);
            }

        }

        public virtual bool OnCollision(GameObject obj)
        {

            if(this != obj)
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

    public struct Vector2
    {
        public float x;
        public float y;

        // Constructor to initialize the vector
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // Overriding ToString() method for easy debugging
        public override string ToString()
        {
            return $"({x}, {y})";
        }

        // Vector addition
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        // Vector subtraction
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        // Scalar multiplication
        public static Vector2 operator *(Vector2 a, float scalar)
        {
            return new Vector2(a.x * scalar, a.y * scalar);
        }

        // Dot product
        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        // Magnitude (length) of the vector
        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        // Normalize the vector (make it unit length)
        public Vector2 Normalize()
        {
            float magnitude = Magnitude();
            return new Vector2(x / magnitude, y / magnitude);
        }

        // Angle between two vectors
        public static float Angle(Vector2 a, Vector2 b)
        {
            return (float)Math.Acos(Dot(a, b) / (a.Magnitude() * b.Magnitude()));
        }

        // Project a vector onto another vector
        public static Vector2 Project(Vector2 a, Vector2 b)
        {
            return b * (Dot(a, b) / Dot(b, b));
        }

        // Reflect a vector off a surface with a given normal
        public static Vector2 Reflect(Vector2 a, Vector2 normal)
        {
            return a - normal * 2 * Dot(a, normal);
        }

        // Lerp (linearly interpolate) between two vectors
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return a * (1 - t) + b * t;
        }

        // Slerp (spherically interpolate) between two vectors
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




    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        // Constructor to initialize the vector
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // Overriding ToString() method for easy debugging
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        // Vector addition
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        // Vector subtraction
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        // Scalar multiplication
        public static Vector3 operator *(Vector3 a, float scalar)
        {
            return new Vector3(a.x * scalar, a.y * scalar, a.z * scalar);
        }

        // Dot product
        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        // Magnitude (length) of the vector
        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        // Normalize the vector (make it unit length)
        public Vector3 Normalize()
        {
            float magnitude = Magnitude();
            return new Vector3(x / magnitude, y / magnitude, z / magnitude);
        }

        // Cross product
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y * b.z - a.z * b.y,
                               a.z * b.x - a.x * b.z,
                               a.x * b.y - a.y * b.x);
        }

        // Angle between two vectors
        public static float Angle(Vector3 a, Vector3 b)
        {
            return (float)Math.Acos(Dot(a, b) / (a.Magnitude() * b.Magnitude()));
        }

        // Project a vector onto another vector
        public static Vector3 Project(Vector3 a, Vector3 b)
        {
            return b * (Dot(a, b) / Dot(b, b));
        }

        // Reflect a vector off a surface with a given normal
        public static Vector3 Reflect(Vector3 a, Vector3 normal)
        {
            return a - normal * 2 * Dot(a, normal);
        }

        // Lerp (linearly interpolate) between two vectors
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return a * (1 - t) + b * t;
        }

        // Slerp (spherically interpolate) between two vectors
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
