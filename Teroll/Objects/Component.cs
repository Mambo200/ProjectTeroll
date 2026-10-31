using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Worlds;

namespace Teroll.Objects
{
    public abstract class Component
    {
        public GameObject Owner;

        public virtual void Update(GameTime gameTime, Level level) { }
    }

}
