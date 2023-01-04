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

        //texte début/fin
        public SpriteFont _fontTest;
        public Vector2 _textPos;

        //boitedialogue
        private Texture2D _dialBox;
        private Vector2 _positionBox;
        private Vector2 _positionDial;
        private bool _dialTrue;

        //Cooldown
        private float _cooldown;
        private bool _cooldownVerif;

        public black_jack(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _textPos = new Vector2(50, 50);
            _positionBox = new Vector2(0, 348);
            _positionDial = new Vector2(105, 365);
            _dialTrue = false;

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fontTest = Content.Load<SpriteFont>("font/font_test");
            _dialBox = Content.Load<Texture2D>("img/dialogue/dialogue_box");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //changements maps, tout premier dialogue

            if (keyboardState.IsKeyDown(Keys.W) && _cooldownVerif == false && _dialTrue == false)
            {
                _dialTrue = true;
                _cooldownVerif = true;
                _cooldown = 0.2f;

            }

            if (keyboardState.IsKeyDown(Keys.W) && _cooldownVerif == false && _dialTrue == true)
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

            if (_dialTrue == false)
            _spriteBatch.DrawString(_fontTest, "Wake the fuck up Samurai, we've got a city to burn", _textPos, Color.White);

            if (_dialTrue == true)
            {
                _spriteBatch.Draw(_dialBox, _positionBox, Color.White);
                _spriteBatch.DrawString(_fontTest, "EH OH GAMIN, REVEIL-TOI! TU VAS M'FAIRE ATTENDRE\nENCORE LONGTEMPS?!", _positionDial, Color.White);
            }              

            _spriteBatch.End();
        }
    }
}
