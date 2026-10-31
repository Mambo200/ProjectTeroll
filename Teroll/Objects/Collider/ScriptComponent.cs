using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Worlds;

namespace Teroll.Objects.Collider
{
    /// <summary>
    /// For moving platforms, boss mechanics, animations, lever logic.
    /// </summary>
    public class ScriptComponent : Component
    {
        public Action<GameObject, GameTime, Level>? Script;

        public override void Update(GameTime gameTime, Level level)
        {
            Script?.Invoke(Owner, gameTime, level);
        }
    }
}
