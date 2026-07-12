using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Worlds;

namespace Teroll.Objects
{
    public abstract class GameObject
    {
        public Vector2 Position;
        public bool IsActive = true;

        public virtual void Update(GameTime gameTime, Level level) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
