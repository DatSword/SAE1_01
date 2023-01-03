using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using AnimatedSprite = MonoGame.Extended.Sprites.AnimatedSprite;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace SAE101
{
    internal class black_jack : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public SpriteFont _fontTest;
        public Vector2 _textPos;
        public black_jack(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _textPos = new Vector2(50, 350);
            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fontTest = Content.Load<SpriteFont>("font/font_test");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your update logic here
            //_tiledMapRenderer.Update(gameTime);

            //changements maps

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                Game.LoadScreenchato_int_chambres_nord();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //_tiledMapRenderer.Draw();
            _spriteBatch.DrawString(_fontTest, "Wake the fuck up Samurai, we've got a city to burn", _textPos, Color.White);
            _spriteBatch.End();
        }
    }
}
