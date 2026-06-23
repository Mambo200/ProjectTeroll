using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Teroll.Entitys;
using Teroll.Tiles;
using Teroll.Worlds;

namespace Teroll
{
    public class Player : Entity
    {
        public Rectangle Collider =>
            new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                16,
                32
            );

        private Texture2D _texture;
        private Vector2 _velocity;

        private bool _jumpPressedLastFrame = false;

        private float _moveSpeed = 200f;
        private float _slowSpeed = 100f;
        private float _gravity = 900f;
        private float _maxYVelocity = 500f;
        private float _jumpStrength = -350f;

        private bool _isOnGround = false;
        private int _jumpsLeft = 2;
        public int maxJumpsAvailable = 2;

        public Player(Texture2D texture, Vector2 startPos)
        {
            _texture = texture;
            Position = startPos;
        }

        // ---------------------------------------------------------
        //  UPDATE (Level → Player)
        // ---------------------------------------------------------
        public override void Update(GameTime gameTime, Level level)
        {
            Tilemap map = level.ActiveScreen.Tilemap;

            Vector2 screenOffset = new Vector2(
                level.CurrentScreenX * Level.ScreenWidthPx,
                level.CurrentScreenY * Level.ScreenHeightPx
            );

            UpdateInternal(gameTime, map, screenOffset);
        }

        // ---------------------------------------------------------
        //  INTERNES UPDATE (Tilemap + Offset)
        // ---------------------------------------------------------
        private void UpdateInternal(GameTime gameTime, Tilemap map, Vector2 screenOffset)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var k = Keyboard.GetState();

            // ---------------------------------------------------------
            // Movement Input
            // ---------------------------------------------------------
            float speed = k.IsKeyDown(Keys.LeftShift) ? _slowSpeed : _moveSpeed;

            if (k.IsKeyDown(Keys.Left))
                _velocity.X = -speed;
            else if (k.IsKeyDown(Keys.Right))
                _velocity.X = speed;
            else
                _velocity.X = 0;

            // ---------------------------------------------------------
            // Jump Input
            // ---------------------------------------------------------
            bool jumpPressed = k.IsKeyDown(Keys.Space);

            if (jumpPressed && !_jumpPressedLastFrame)
            {
                if (_isOnGround)
                {
                    _velocity.Y = _jumpStrength;
                    _isOnGround = false;
                    _jumpsLeft = maxJumpsAvailable - 1;
                }
                else if (_jumpsLeft > 0)
                {
                    _velocity.Y = _jumpStrength;
                    _jumpsLeft--;
                }
            }

            if (!jumpPressed && _velocity.Y < 0)
                _velocity.Y += 20f;

            _jumpPressedLastFrame = jumpPressed;

            // ---------------------------------------------------------
            // 1) Horizontal Movement
            // ---------------------------------------------------------
            Position.X += _velocity.X * dt;

            var tileX = GetCollidingTileLocal(map, screenOffset);
            if (tileX != null)
            {
                if (_velocity.X > 0)
                    Position.X = tileX.Collider.Left + screenOffset.X - Collider.Width;
                else if (_velocity.X < 0)
                    Position.X = tileX.Collider.Right + screenOffset.X;

                _velocity.X = 0;
            }

            // ---------------------------------------------------------
            // 2) Vertical Movement
            // ---------------------------------------------------------
            Position.Y += _velocity.Y * dt;

            var tileY = GetCollidingTileLocal(map, screenOffset);
            if (tileY != null)
            {
                if (_velocity.Y > 0)
                {
                    Position.Y = tileY.Collider.Top + screenOffset.Y - Collider.Height;
                    _isOnGround = true;
                    _jumpsLeft = maxJumpsAvailable;
                }
                else if (_velocity.Y < 0)
                {
                    Position.Y = tileY.Collider.Bottom + screenOffset.Y;
                }

                _velocity.Y = 0;
            }
            else
            {
                const int snapDistance = 3;

                Rectangle probe = Collider;
                probe.Y += snapDistance;

                var snapTile = GetCollidingTileLocal(map, screenOffset, probe);

                if (snapTile != null && _velocity.Y >= 0)
                {
                    Position.Y = snapTile.Collider.Top + screenOffset.Y - Collider.Height;
                    _isOnGround = true;
                    _velocity.Y = 0;
                    _jumpsLeft = maxJumpsAvailable;
                }
                else
                {
                    if (_isOnGround)
                        _jumpsLeft--;
                    _isOnGround = false;
                }
            }

            // ---------------------------------------------------------
            // 3) Gravity
            // ---------------------------------------------------------
            if (!_isOnGround)
            {
                _velocity.Y += _gravity * dt;
                _velocity.Y = Math.Min(_velocity.Y, _maxYVelocity);
            }
            else
            {
                _velocity.Y = 0;
            }
        }

        // ---------------------------------------------------------
        //  TILE COLLISION (lokale Koordinaten!)
        // ---------------------------------------------------------
        private Tile GetCollidingTileLocal(Tilemap map, Vector2 offset, Rectangle? customRect = null)
        {
            Rectangle worldRect = customRect ?? Collider;

            // Welt → lokale Screen-Koordinaten
            Rectangle localRect = new Rectangle(
                (int)(worldRect.X - offset.X),
                (int)(worldRect.Y - offset.Y),
                worldRect.Width,
                worldRect.Height
            );

            foreach (var tile in map.GetNearbyTiles(localRect))
            {
                if (tile.IsSolid && localRect.Intersects(tile.Collider))
                    return tile;
            }

            return null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, new Vector2(.5f, 1), SpriteEffects.None, 0);
        }
    }
}
