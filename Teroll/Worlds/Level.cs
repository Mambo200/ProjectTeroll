using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Teroll.Entitys;

namespace Teroll.Worlds
{
    public class Level
    {
        public Screen[,] Screens;

        public int ScreensX;
        public int ScreensY;

        public const int ScreenWidthPx = Teroll.Tiles.Tilemap.ScreenWidthTiles* Teroll.Tiles.Tile.Size;
        public const int ScreenHeightPx = Teroll.Tiles.Tilemap.ScreenHeightTiles * Teroll.Tiles.Tile.Size;

        public int CurrentScreenX = 0;
        public int CurrentScreenY = 0;

        public List<Entity> GlobalEntities = new List<Entity>();

        public Level(int screensX, int screensY)
        {
            ScreensX = screensX;
            ScreensY = screensY;

            Screens = new Screen[screensX, screensY];
        }

        public Screen ActiveScreen =>
            Screens[CurrentScreenX, CurrentScreenY];

        public void Update(GameTime gameTime)
        {
            foreach (var e in GlobalEntities)
                e.Update(gameTime, this);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileTexture)
        {
            for (int sy = 0; sy < ScreensY; sy++)
            {
                for (int sx = 0; sx < ScreensX; sx++)
                {
                    Screen screen = Screens[sx, sy];
                    if (screen == null) continue;

                    Vector2 offset = new Vector2(
                        sx * ScreenWidthPx,
                        sy * ScreenHeightPx
                    );

                    screen.Tilemap.Draw(spriteBatch, offset, tileTexture);

                    foreach (var e in screen.LocalEntities)
                        e.Draw(spriteBatch);
                }
            }

            foreach (var e in GlobalEntities)
                e.Draw(spriteBatch);
        }
    }

}
