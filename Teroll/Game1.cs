using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Teroll
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Player Player { get; private set; }
        private Texture2D _playerTexture;

#if DEBUG
        private SpriteFont _debugFont;
#endif
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsFixedTimeStep = true;
            //TargetElapsedTime = TimeSpan.FromSeconds(1d / 60);
            
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _playerTexture = Content.Load<Texture2D>("Player\\Player");
            Player = new Player(_playerTexture, new Vector2(100, 100));

#if DEBUG
            _debugFont = Content.Load<SpriteFont>("Debug\\DebugFont");
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                maxFPS = double.MinValue;
                minFPS = double.MaxValue;
            }
#endif
            // TODO: Add your update logic here
            Player.Update(gameTime);

            base.Update(gameTime);
        }
#if DEBUG
        // MAX MIN FPS
        double maxFPS = double.MinValue;
        double minFPS = double.MaxValue;
#endif
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            Player.Draw(_spriteBatch);
#if DEBUG
            double currentFPS = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            if(currentFPS > maxFPS)
                maxFPS = currentFPS;
            if(currentFPS < minFPS)
                minFPS = currentFPS;
            _spriteBatch.DrawString(
                _debugFont,
                "Current " + ((int)(1 / gameTime.ElapsedGameTime.TotalSeconds)).ToString() + "\nMin " + ((int)minFPS).ToString() + "\nMax " + ((int)maxFPS).ToString(),
                new Vector2(10, 10),
                Color.White);
#endif

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
