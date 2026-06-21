using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll.Tiles
{
    public abstract class Tile
    {
        public Vector2 Position;
        public const int Size = 32;

        public Rectangle Collider =>
            new Rectangle((int)Position.X, (int)Position.Y, Size, Size);

        public virtual bool IsSolid => false;

        public Tile(int x, int y)
        {
            Position = new Vector2(x, y);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Collider, Color.White);
        }

    }
}
