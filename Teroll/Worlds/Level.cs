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

        public Vector2 CameraPosition =>
            new Vector2(
                CurrentScreenX * ScreenWidthPx,
                CurrentScreenY * ScreenHeightPx
            );


        public Screen ActiveScreen =>
            Screens[CurrentScreenX, CurrentScreenY];

        public void Update(GameTime gameTime)
        {
            foreach (var e in GlobalEntities)
                e.Update(gameTime, this);

            TryScreenTransition(Game1.Get.Player);
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

        public void TryScreenTransition(Player player)
        {
            float relativeX = player.Position.X - (CurrentScreenX * ScreenWidthPx);
            float relativeY = player.Position.Y - (CurrentScreenY * ScreenHeightPx);

            // --- Rechts raus ---
            if (relativeX > ScreenWidthPx)
            {
                if (CurrentScreenX + 1 < ScreensX)
                {
                    CurrentScreenX++;
                    player.Position.X = CurrentScreenX * ScreenWidthPx + 1;
                }
                else
                {
                    player.Position.X = CurrentScreenX * ScreenWidthPx + ScreenWidthPx - 1;
                }
            }

            // --- Links raus ---
            if (relativeX < 0)
            {
                if (CurrentScreenX - 1 >= 0)
                {
                    CurrentScreenX--;
                    player.Position.X = (CurrentScreenX + 1) * ScreenWidthPx - player.Collider.Width - 1;
                }
                else
                {
                    player.Position.X = CurrentScreenX * ScreenWidthPx + 1;
                }
            }

            // --- Unten raus ---
            if (relativeY > ScreenHeightPx)
            {
                if (CurrentScreenY + 1 < ScreensY)
                {
                    CurrentScreenY++;
                    player.Position.Y = CurrentScreenY * ScreenHeightPx + 1;
                }
                else
                {
                    player.Position.Y = CurrentScreenY * ScreenHeightPx + ScreenHeightPx - 1;
                }
            }

            // --- Oben raus ---
            if (relativeY < 0)
            {
                if (CurrentScreenY - 1 >= 0)
                {
                    CurrentScreenY--;
                    player.Position.Y = (CurrentScreenY + 1) * ScreenHeightPx - player.Collider.Height - 1;
                }
                else
                {
                    player.Position.Y = CurrentScreenY * ScreenHeightPx + 1;
                }
            }
        }

    }

}
