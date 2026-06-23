using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Entitys;
using Teroll.Tiles;

namespace Teroll.Worlds
{
    public class Screen
    {
        public Tilemap Tilemap;
        public List<Entity> LocalEntities = new List<Entity>();

        public bool IsDynamicSize = false; // Standard: statisch
        public Vector2 CameraPosition = Vector2.Zero;

        public Screen(Tilemap map, bool dynamicSize = false)
        {
            Tilemap = map;
            IsDynamicSize = dynamicSize;
        }
    }


}
