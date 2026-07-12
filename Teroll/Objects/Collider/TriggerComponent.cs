using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Worlds;

namespace Teroll.Objects.Collider
{
    /// <summary>
    /// For levers, checkpoints or events
    /// </summary>
    public class TriggerComponent : Component
    {
        public Action<Player>? OnTrigger;

        public override void Update(GameTime gameTime, Level level)
        {
            var player = Game1.Get.Player;

            if (player.Collider.Intersects(
                new Rectangle(
                    (int)Owner.Position.X,
                    (int)Owner.Position.Y,
                    32, 32)))
            {
                OnTrigger?.Invoke(player);
            }
        }
    }
}
