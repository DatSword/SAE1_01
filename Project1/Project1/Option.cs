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
using static System.Formats.Asn1.AsnWriter;

namespace SAE101
{
    public class Option : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Titre
        private Texture2D _titleS;
        private SpriteFont _fontTitle;

        // dialbox
        private Texture2D _optBox;
        private Vector2 _posOptBox;
        private String[] _textOpt;
        private Vector2[] _posTextOpt;
        private Texture2D[] _touchesOpt;
        private Vector2[] _posTouches;
        private String _textDep;


        public Option(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _posOptBox = new Vector2(0, 224);

            _textOpt = new String[3] { "pour valider", "pour revenir en arrière", "pour les déplacements" };
            _posTextOpt = new Vector2[3] { new Vector2(100, 260), new Vector2(100, 290), new Vector2(100, 320) };

            _touchesOpt = new Texture2D[3] { Content.Load<Texture2D>("menu/toucheX"), Content.Load<Texture2D>("menu/toucheX"), Content.Load<Texture2D>("menu/toucheX") };
            _posTouches = new Vector2[3] { new Vector2(40, 250), new Vector2(40, 280), new Vector2(20, 310) };

            _textDep = "hi";

                //"← ↑ ↓ →";

            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _titleS = Content.Load<Texture2D>("menu/tantopie");
            _fontTitle = Content.Load<SpriteFont>("font/fonttitle");

            _optBox = Content.Load<Texture2D>("menu/options_box");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            var transformMatrix = Game1._camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _spriteBatch.Draw(_titleS, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(_fontTitle, "Tantopie", new Vector2(10, 0), Color.Gray);
            _spriteBatch.Draw(_optBox, _posOptBox, Color.White);

            for (int i = 0; i < _textOpt.Length; i++)
                _spriteBatch.DrawString(Game1._font, _textOpt[i], _posTextOpt[i], Color.White);
            for (int i = 0; i < _textOpt.Length; i++)
                _spriteBatch.Draw(_touchesOpt[i], _posTouches[i], Color.White);


            _spriteBatch.End();

        }
    }
}
