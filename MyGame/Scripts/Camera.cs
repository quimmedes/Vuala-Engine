using EngineCSharp.Vuala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static EngineCSharp.Vuala.App;


namespace EngineCSharp.MyGame.Scripts
{
    public  class Camera : GameObject
    {
        private GameObject target;

        // Camera position offset from target
        private Vector3 offset = new Vector3(0,0,0);

        // Smoothing factor (lower = smoother, higher = more responsive)
        private float smoothing = 0.125f;

        // Bounds to restrict camera movement
        private bool useBounds = true;
        private float minX = 0;
        private float maxX = App.Window_Width;
        private float minY = 0;
        private float maxY = App.Window_Height;

        // Camera dimensions (viewport)
        private int viewportWidth;
        private int viewportHeight;

        // Current camera position
        private Vector3 position = new Vector3(0, 0,-10);

        // Singleton instance
        public static Camera Instance { get; private set; }



        /// <summary>
        /// Initializes the camera component.
        /// </summary>
        /// 
 
        public override void Start()
        {
             
            base.Start();

            texture = LoadTextureFromMemory(render, Properties.Resources.background);
            transform.position.x = 0;
            transform.position.y = 0;

            // Set up the singleton instance
            Instance = this;

            // Set viewport dimensions to window size by default
            viewportWidth = App.Window_Width;
            viewportHeight = App.Window_Height;

            // Try to find the player if not set
            if (target == null)
            {
                target = Player.player;
            }

            // Initialize position if target exists
            if (target != null)
            {

              

                // Apply bounds
                if (useBounds)
                {
                    ApplyBounds();
                }
            }
        }

        /// <summary>
        /// Updates the camera position to follow the target with smoothing.
        /// </summary>
        public override void Update()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.transform.position - offset;
                
                offset = Vector3.Lerp(offset, desiredPosition, 0.125f);

                position = offset;
                transform.position = position;

                // Apply bounds
                if (useBounds)
                {
                    ApplyBounds();
                }


            }
          
        }

        /// <summary>
        /// Sets the target for the camera to follow.
        /// </summary>
        /// <param name="newTarget">The GameObject to follow.</param>
        public void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }

        /// <summary>
        /// Sets the offset from the target's center.
        /// </summary>
        /// <param name="x">X offset.</param>
        /// <param name="y">Y offset.</param>
        public void SetOffset(float x, float y)
        {
            offset.x = x;
            offset.y = y;
        }

        /// <summary>
        /// Sets how smoothly the camera follows the target.
        /// Lower values = smoother but slower, higher values = more responsive but jerkier.
        /// </summary>
        /// <param name="value">Smoothing factor (0.01f to 1.0f)</param>
        public void SetSmoothing(float value)
        {
            smoothing = Math.Clamp(value, 0.01f, 1.0f);
        }

        /// <summary>
        /// Sets the camera bounds to restrict its movement.
        /// </summary>
        /// <param name="minX">Minimum X position.</param>
        /// <param name="minY">Minimum Y position.</param>
        /// <param name="maxX">Maximum X position.</param>
        /// <param name="maxY">Maximum Y position.</param>
        public void SetBounds(float minX, float minY, float maxX, float maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX - viewportWidth;
            this.maxY = maxY - viewportHeight;
            useBounds = true;
        }

        /// <summary>
        /// Disables camera bounds checking.
        /// </summary>
        public void DisableBounds()
        {
            useBounds = false;
        }

        /// <summary>
        /// Gets the current camera position.
        /// </summary>
        /// <returns>The camera position as a Vector2.</returns>
        public Vector3 GetPosition()
        {
            return offset;
        }

        /// <summary>
        /// Applies boundary restrictions to the camera position.
        /// </summary>
        private void ApplyBounds()
        {
            position.x = Math.Clamp(position.x, minX, maxX);
            position.y = Math.Clamp(position.y, minY, maxY);
        }

        /// <summary>
        /// Converts world coordinates to screen coordinates based on camera position.
        /// </summary>
        /// <param name="worldX">World X coordinate.</param>
        /// <param name="worldY">World Y coordinate.</param>
        /// <returns>Screen coordinates as a Vector2.</returns>
        public Vector2 WorldToScreenPoint(float worldX, float worldY)
        {
            return new Vector2(worldX - position.x, worldY - position.y);
        }

        /// <summary>
        /// Converts screen coordinates to world coordinates based on camera position.
        /// </summary>
        /// <param name="screenX">Screen X coordinate.</param>
        /// <param name="screenY">Screen Y coordinate.</param>
        /// <returns>World coordinates as a Vector2.</returns>
        public Vector2 ScreenToWorldPoint(float screenX, float screenY)
        {
            return new Vector2(screenX + position.x, screenY + position.y);
        }
    }

    /// <summary>
    /// Helper class for math operations.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        /// <param name="start">Start value.</param>
        /// <param name="end">End value.</param>
        /// <param name="amount">Interpolation amount (0.0 to 1.0).</param>
        /// <returns>Interpolated value.</returns>
        public static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }
    }


}
