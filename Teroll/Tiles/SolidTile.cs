using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll.Tiles
{
    public class SolidTile : Tile
    {
        public override bool IsSolid => true;

        public SolidTile(int x, int y) : base(x, y) { }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Collider, Color.Gray); // Beispiel
        }
    }
}
