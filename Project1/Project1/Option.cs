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

        // pour récupérer une référence à l’objet game pour avoir accès à tout ce qui est défini dans Game1
        private Game1 _myGame;

        //Titre
        private Texture2D _titleS;
        private SpriteFont _fontTitle;

        // dialbox
        private Texture2D _optBox;
        private Vector2 _posOptBox;


        public Option(Game1 game) : base(game) { }

        public override void Initialize()
        {
            _posOptBox = new Vector2(0, 228);

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
            _spriteBatch.DrawString(_fontTitle, "Tantopie", new Vector2(10, 0), Color.White);
            _spriteBatch.Draw(_optBox, _posOptBox, Color.White);

            _spriteBatch.End();

        }
    }
}
