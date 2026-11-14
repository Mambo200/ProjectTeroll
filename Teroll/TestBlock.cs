using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Objects;

namespace Teroll
{
    public class TestBlock : GameObject
    {
        public override Rectangle Collider =>
            new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                32,
                32);

        public TestBlock(Vector2 position)
        {
            Position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Helper.TextureLibrary.Platform,
                Collider,
                Color.White);
        }
    }
}
