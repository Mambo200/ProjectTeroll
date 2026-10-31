using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll.Objects.Collider
{
    /// <summary>
    /// For collision with player or other objects
    /// </summary>
    public class ColliderComponent : Component
    {
        public Rectangle Bounds => new Rectangle(
            (int)Owner.Position.X,
            (int)Owner.Position.Y,
            Width,
            Height
        );

        public int Width;
        public int Height;

        public ColliderComponent(GameObject owner, int width, int height)
        {
            Owner = owner;
            Width = width;
            Height = height;
        }
    }

}
