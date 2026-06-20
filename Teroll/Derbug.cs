using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll
{
    public static class Derbug
    {
        private static SpriteBatch batch;
        private static SpriteFont font;
        private static string text = string.Empty;
        public static void Init(SpriteBatch _batch, SpriteFont _font)
        {
            batch = _batch;
            font = _font;
        }

        public static void SetText(string _text)
        {
            text = _text;
        }

        public static void Draw()
        {
            batch.DrawString(
                font,
                text,
                new Vector2(10, 10),
                Color.White);
        }
    }
}
