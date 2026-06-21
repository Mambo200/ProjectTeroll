using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll.Tiles
{
    public class Tilemap
    {
        public Tile[,] Tiles;
        public const int TileSize = 32;
        public const int ScreenWidthTiles = 25;
        public const int ScreenHeightTiles = 19;

        public Tilemap(int[,] data)
        {
            Tiles = new Tile[ScreenWidthTiles, ScreenHeightTiles];

            for (int y = 0; y < ScreenHeightTiles; y++)
            {
                for (int x = 0; x < ScreenWidthTiles; x++)
                {
                    int value = data[y, x]; // hier der entscheidende Fix

                    if (value == 1)
                        Tiles[x, y] = new SolidTile(x * Tile.Size, y * Tile.Size);
                    else
                        Tiles[x, y] = null;
                }
            }
        }

        public IEnumerable<Tile> GetNearbyTiles(Rectangle area)
        {
            int left = area.Left / Tile.Size;
            int right = area.Right / Tile.Size;
            int top = area.Top / Tile.Size;
            int bottom = area.Bottom / Tile.Size;

            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    if (x < 0 || y < 0 || x >= ScreenWidthTiles || y >= ScreenHeightTiles)
                        continue;

                    if (Tiles[x, y] != null)
                        yield return Tiles[x, y];
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D solidTexture)
        {
            for (int y = 0; y < ScreenHeightTiles; y++)
            {
                for (int x = 0; x < ScreenWidthTiles; x++)
                {
                    Tile tile = Tiles[x, y];
                    if (tile != null)
                    {
                        tile.Draw(spriteBatch, solidTexture);
                    }
                }
            }
        }

    }
}
