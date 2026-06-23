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
        public string DEBUGNAME;
        public Tile[,] Tiles;
        public const int ScreenWidthTiles = 25;
        public const int ScreenHeightTiles = 19;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public Tilemap(int[,] data)
        {
            Height = data.GetLength(0);
            Width = data.GetLength(1);

            Tiles = new Tile[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int value = data[y, x];

                    if (value == 1)
                        Tiles[x, y] = new SolidTile(x * Tile.Size, y * Tile.Size);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tile t = Tiles[x, y];
                    if (t == null) continue;

                    Rectangle dest = t.Collider;
                    dest.X += (int)offset.X;
                    dest.Y += (int)offset.Y;

                    spriteBatch.Draw(texture, dest, Color.White);
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
                    if (x < 0 || y < 0 || x >= Width || y >= Height)
                        continue;

                    if (Tiles[x, y] != null)
                        yield return Tiles[x, y];
                }
            }
        }
    }
}
