using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Worlds;

namespace Teroll.Entitys
{
    public abstract class Entity
    {
        public Vector2 Position;

        public abstract void Update(GameTime gameTime, Level level);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
