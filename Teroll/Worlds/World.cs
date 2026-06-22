using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll.Worlds
{
    public class World
    {
        public List<Level> Levels = new List<Level>();
        public int CurrentLevel = 0;

        public Level ActiveLevel => Levels[CurrentLevel];

        public void Update(GameTime gameTime)
        {
            ActiveLevel.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileTexture)
        {
            ActiveLevel.Draw(spriteBatch, tileTexture);
        }
    }

}
