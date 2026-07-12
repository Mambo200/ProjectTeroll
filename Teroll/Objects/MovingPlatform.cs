using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Objects.Collider;
using Teroll.Worlds;

namespace Teroll.Objects
{
    public class MovingPlatform : GameObject
    {
        private ColliderComponent collider;
        private ScriptComponent script;

        public MovingPlatform(Vector2 startPos)
        {
            Position = startPos;

            collider = new ColliderComponent(this, 32, 8);
            script = new ScriptComponent
            {
                Owner = this,
                Script = (obj, time, level) =>
                {
                    float t = (float)time.TotalGameTime.TotalSeconds;
                    obj.Position.X = startPos.X + (float)Math.Sin(t) * 50f;
                }
            };
        }

        public override void Update(GameTime gameTime, Level level)
        {
            collider.Update(gameTime, level);
            script.Update(gameTime, level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureLibrary.Platform, Position, Color.White);
        }
    }
}
