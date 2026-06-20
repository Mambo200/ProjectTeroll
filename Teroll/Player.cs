using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Teroll
{
    public class Player
    {
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

        public Player(Texture2D texture, Vector2 startPos)
        {
            _texture = texture;
            _position = startPos;
        }

        public void Update(GameTime gameTime)
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
                _velocity.X = 0; // Sofort stehen bleiben

            // --- Jump ---
            if (k.IsKeyDown(Keys.Space) && _isOnGround)
            {
                _velocity.Y = _jumpStrength;
                _isOnGround = false;
                _jumpsLeft = 1; // Double-Jump übrig
            }
            else if (k.IsKeyDown(Keys.Space) && _jumpsLeft > 0 && _velocity.Y > 0)
            {
                _velocity.Y = _jumpStrength;
                _jumpsLeft--;
            }

            // --- Gravity ---
            _velocity.Y += _gravity * dt;
            _velocity.Y = Math.Min(_velocity.Y, _maxYVelovity);

            // --- Apply movement ---
            _position += _velocity * dt;

            // --- Simple ground collision (Test) ---
            if (_position.Y >= 400)
            {
                _position.Y = 400;
                _velocity.Y = 0;
                _isOnGround = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);

        }
    }
}