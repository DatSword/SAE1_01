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
    internal class ecran_de_titre : GameScreen
    {
        //map
        private new Game1 Game => (Game1)base.Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TiledMap _tiledMap;
        //private TiledMapRenderer _tiledMapRenderer;

        public SpriteFont _fontTest;


        public ecran_de_titre(Game1 game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //_tiledMap = Content.Load<TiledMap>("map/chato/tmx/chato_ext_cours_interieur");
            //_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
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
                Game.LoadScreenblack_jack();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BlueViolet);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //_tiledMapRenderer.Draw();
            _spriteBatch.DrawString(_fontTest, "Jeu de Fou",new Vector2(0,0), Color.White);
            _spriteBatch.End();
        }
    }
}
