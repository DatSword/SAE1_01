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
        //"map"
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Vector2 _textPos;

        //Cooldown
        private float _cooldown;
        private bool _cooldownVerif;

        public black_jack(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _textPos = new Vector2(50, 50);

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //changements maps, tout premier dialogue

            if (keyboardState.IsKeyDown(Keys.W) && _cooldownVerif == false && Game1._dialTrue == false)
            {
                eventsetdial.Event1();
                _cooldownVerif = true;
                _cooldown = 0.2f;

            }

            if (keyboardState.IsKeyDown(Keys.W) && _cooldownVerif == false && Game1._dialTrue == true)
            {
                Game.LoadScreenchato_int_chambres_nord();
            }

            if (_cooldownVerif == true)
            {
                _cooldown = _cooldown - deltaSeconds;
                if (_cooldown <= 0)
                    _cooldownVerif = false;
            }


        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            if (Game1._dialTrue == false)
            _spriteBatch.DrawString(Game1._font, "Wake the fuck up Samurai, we've got a city to burn", _textPos, Color.White);

            if (Game1._dialTrue == true)
            {
                _spriteBatch.Draw(Game1._dialBox, Game1._posDialBox, Color.White);
                _spriteBatch.DrawString(Game1._font, Game1._text, Game1._posText, Color.White);
                _spriteBatch.DrawString(Game1._font, Game1._nom, Game1._posNom, Color.White);
            }
            _spriteBatch.End();
        }
    }
}
