using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Teroll.Entitys;
using Teroll.Tiles;
using Teroll.Worlds;

namespace Teroll
{
    public class Player : Entity
    {
        public Rectangle Collider
        {
            get
            {
                return new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    16,
                    32
                    );
            }
        }

        private bool _jumpPressedLastFrame = false;
        private float _jumpCutMultiplier = 0.95f; // wie stark der Sprung gekürzt wird

        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;

        private float _moveSpeed = 200f;       // Sofort Max-Speed
        private float _slowSpeed = 100f;       // Shift gedrückt
        private float _gravity = 900f;
        private float _maxYVelovity = 500;
        private float _jumpStrength = -350f;

        private bool _isOnGround = false;
        private int _jumpsLeft = 2;            // Double-Jump
        public int maxJumpsAvailable = 2;

        public Player(Texture2D texture, Vector2 startPos)
        {
            _texture = texture;
            _position = startPos;
        }

        public override void Update(GameTime gameTime, Level level)
        {
            Update(gameTime, level.ActiveScreen.Tilemap);
        }

        public void Update(GameTime gameTime, Tilemap map)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var k = Keyboard.GetState();

            // --- Movement ---
            float speed = k.IsKeyDown(Keys.LeftShift) ? _slowSpeed : _moveSpeed;

            if (k.IsKeyDown(Keys.Left))
                _velocity.X = -speed;
            else if (k.IsKeyDown(Keys.Right))
                _velocity.X = speed;
            else
                _velocity.X = 0;

            // --- Jump Input ---
            bool jumpPressed = k.IsKeyDown(Keys.Space);

            if (jumpPressed && !_jumpPressedLastFrame)
            {
                if (_isOnGround)
                {
                    _velocity.Y = _jumpStrength;
                    _isOnGround = false;
                    _jumpsLeft--;
                }
                else if (_jumpsLeft > 0)
                {
                    _velocity.Y = _jumpStrength;
                    _jumpsLeft--;
                }
            }

            // Variable Sprunghöhe
            if (!jumpPressed && _velocity.Y < 0)
                _velocity.Y += 20f;

            _jumpPressedLastFrame = jumpPressed;

            // ---------------------------------------------------------
            // 1) Horizontal bewegen (OHNE Gravity)
            // ---------------------------------------------------------
            _position.X += _velocity.X * dt;

            var tileX = GetCollidingTile(Collider, map);
            if (tileX != null)
            {
                if (_velocity.X > 0)
                    _position.X = tileX.Collider.Left - Collider.Width;
                else if (_velocity.X < 0)
                    _position.X = tileX.Collider.Right;

                _velocity.X = 0;
            }

            // ---------------------------------------------------------
            // 2) Vertikal bewegen (OHNE Gravity)
            // ---------------------------------------------------------
            _position.Y += _velocity.Y * dt;

            var tileY = GetCollidingTile(Collider, map);
            if (tileY != null)
            {
                if (_velocity.Y > 0)
                {
                    // normal landen
                    _position.Y = tileY.Collider.Top - Collider.Height;
                    _isOnGround = true;
                    _jumpsLeft = maxJumpsAvailable;
                }
                else if (_velocity.Y < 0)
                {
                    // Kopf stößt an Decke
                    _position.Y = tileY.Collider.Bottom;
                }

                _velocity.Y = 0;
            }
            else
            {
                // kein direkter Kontakt → prüfen, ob wir knapp über dem Boden sind
                const int snapDistance = 3; // Toleranz in Pixeln

                Rectangle probe = Collider;
                probe.Y += snapDistance;

                var snapTile = GetCollidingTile(probe, map);

                if (snapTile != null && _velocity.Y >= 0)
                {
                    // wir sind maximal snapDistance über dem Boden → hart aufsetzen
                    _position.Y = snapTile.Collider.Top - Collider.Height;
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
            // 3) Gravity NACH der Kollision anwenden
            // ---------------------------------------------------------
            if (!_isOnGround)
            {
                _velocity.Y += _gravity * dt;
                _velocity.Y = Math.Min(_velocity.Y, _maxYVelovity);
            }
            else
            {
                _velocity.Y = 0; // WICHTIG: verhindert das Flackern
            }

            Derbug.SetText(_isOnGround.ToString() + " | " + _jumpsLeft.ToString() + " | " + ((int)_velocity.Y).ToString());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, new Vector2(.5f,1), SpriteEffects.None, 0);

        }

        private Tile GetCollidingTile(Rectangle rect, Tilemap map)
        {
            foreach (var tile in map.GetNearbyTiles(rect))
            {
                if (tile.IsSolid && rect.Intersects(tile.Collider))
                    return tile;
            }

            return null;
        }


    }
}