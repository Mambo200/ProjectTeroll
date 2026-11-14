using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Objects;
using Teroll.Worlds;

namespace Teroll.Entitys
{
    public abstract class Entity
    {
        public Vector2 Position;

        public abstract void Update(GameTime gameTime, Level level);
        public abstract void Draw(SpriteBatch spriteBatch);

        protected bool CanHitLeft(GameObject obj)
        {
            return obj.CollisionSides.HasFlag(
                CollisionSide.Left);
        }

        protected bool CanHitRight(GameObject obj)
        {
            return obj.CollisionSides.HasFlag(
                CollisionSide.Right);
        }

        protected bool CanHitBottom(GameObject obj)
        {
            return obj.CollisionSides.HasFlag(
                CollisionSide.Bottom);
        }

        protected bool CanHitTop(GameObject obj)
        {
            return obj.CollisionSides.HasFlag(
                CollisionSide.Top);
        }
    }
}
