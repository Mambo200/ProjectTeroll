using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll
{
    public class Tilemap
    {
        public const int TileSize = 32;
        public const int ScreenWidthTiles = 25;
        public const int ScreenHeightTiles = 19;


        // 0 = leer, 1 = solid
        public int[,] Tiles;

        public Tilemap(int[,] tiles)
        {
            Tiles = tiles;
        }

        public bool IsSolid(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Tiles.GetLength(0) || y >= Tiles.GetLength(1))
                return false;

            return Tiles[x, y] == 1;
        }
    }
}
