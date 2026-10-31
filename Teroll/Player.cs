using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.Remoting;
using Teroll.Entitys;
using Teroll.Objects;
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

        private Vector2 _previousPosition;

        private float _moveSpeed = 200f;
        private float _slowSpeed = 100f;
        private float _gravity = 900f;
        private float _maxYVelocity = 500f;
        private float _jumpStrength = -350f;
        public int snapDistance = 3;

        private bool _isOnPlatform = false;
        private bool _isOnGround = false;
        private int _jumpsLeft = 2;
        public int maxJumpsAvailable = 5;

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
            _previousPosition = Position;

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

            _isOnGround = false;        // WICHTIG: Reset hier
            _isOnPlatform = false;      // WICHTIG: Reset hier

            var tileY = GetCollidingTileLocal(map, screenOffset);
            if (tileY != null)
            {
                if (_velocity.Y > 0)
                {
                    Position.Y = tileY.Collider.Top + screenOffset.Y - Collider.Height;
                    HitGround();
                }
                else if (_velocity.Y < 0)
                {
                    Position.Y = tileY.Collider.Bottom + screenOffset.Y;
                }

                _velocity.Y = 0;
            }
            else
            {
                // SnapDistance für Tiles
                Rectangle probe = Collider;
                probe.Y += snapDistance;

                var snapTile = GetCollidingTileLocal(map, screenOffset, probe);

                if (snapTile != null && _velocity.Y >= 0)
                {
                    Position.Y = snapTile.Collider.Top + screenOffset.Y - Collider.Height;
                    HitGround();
                }
            }

            // ---------------------------------------------------------
            // 3) Object collision check (MovingPlatform + Object)
            // ---------------------------------------------------------
            var obj = GetCollidingObject(Game1.Get.testLevel);

            if (obj != null)
            {
                
                if (_velocity.X > 0 &&
                    CanHitLeft(obj) &&
                    obj.Position.X > Position.X)
                {
                    Position.X =
                        obj.Collider.Left -
                        Collider.Width;

                    _velocity.X = 0;
                }

                else if (_velocity.X < 0 &&
                    CanHitRight(obj))
                {
                    Position.X =
                        obj.Collider.Right;

                    _velocity.X = 0;
                }

                else if (_velocity.Y > 0 && CanHitTop(obj))
                {
                    Position.Y =
                        obj.Collider.Top - obj.Collider.Height;

                    _velocity.Y = 0;
                }
                else if (_velocity.Y < 0 && CanHitBottom(obj))
                {
                    Position.Y =
                        obj.Collider.Bottom;

                    _velocity.Y = 0;
                }



            }
            else
            {
                // SnapDistance für Plattformen
                Rectangle probe = Collider;
                probe.Y += snapDistance;
                
                var snapPlatform = GetCollidingObject(Game1.Get.testLevel, probe);
                
                if (snapPlatform is MovingPlatform mp && _velocity.Y >= 0)
                {
                    _isOnPlatform = true;
                    Position.Y = mp.Position.Y - Collider.Height;
                    _velocity.Y = 0;
                    _isOnGround = true;
                    _jumpsLeft = maxJumpsAvailable;
                
                    Position.X += mp.Position.X - mp.PreviousPosition.X;
                }
            }



            // ---------------------------------------------------------
            // 4) Gravity
            // ---------------------------------------------------------
            if (!_isOnGround && !_isOnPlatform)
            {
                _velocity.Y += _gravity * dt;
                _velocity.Y = Math.Min(_velocity.Y, _maxYVelocity);
            }
            else
            {
                _velocity.Y = 0;
            }

            Derbug.SetText(_jumpsLeft.ToString());
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

        private GameObject GetCollidingObject(Level level)
        {
            foreach (var obj in level.ActiveScreen.Objects)
            {
                if (Collider.Intersects(obj.Collider))
                    return obj;
            }

            return null;
        }
        private GameObject GetCollidingObject(Level level, Rectangle rect)
        {
            foreach (var obj in level.ActiveScreen.Objects)
            {
                if (rect.Intersects(obj.Collider))
                    return obj;
            }

            return null;
        }

        public void HitGround()
        {
            _isOnGround = true;
            _velocity.Y = 0;
            _jumpsLeft = maxJumpsAvailable;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, new Vector2(.5f, 1), SpriteEffects.None, 0);
        }

        private bool CanLandOn(GameObject obj)
        {
            if (!obj.CollisionSides.HasFlag(CollisionSide.Top))
                return false;

            Rectangle previousCollider =
                new Rectangle(
                    (int)_previousPosition.X,
                    (int)_previousPosition.Y,
                    Collider.Width,
                    Collider.Height);

            return previousCollider.Bottom <= obj.Collider.Top
                   && _velocity.Y >= 0;
        }
    }
}
