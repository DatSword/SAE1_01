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
using System;


namespace SAE101
{
    public class Game1 : Game
    {
        //honnêtement j'en sais r
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;
        private KeyboardState _keyboardState;

        //Musique
        private Song _songChato;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            // Definition écran
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 640;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphics.ApplyChanges();

            base.Initialize();

            //premier écran
            LoadScreenchato_int_chambres_couloir();
        }

        protected override void LoadContent()
        {
            //Jsp
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Musiques
            _songChato = Content.Load<Song>("music/chato/EdgarAndSabin");
            MediaPlayer.Play(_songChato);
        }

        protected override void Update(GameTime gameTime)
        {
            //Mannette?
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _keyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void LoadScreenchato_int_chambres_couloir()
        {
            _screenManager.LoadScreen(new chato_int_chambres_couloir(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreenchato_int_chambres_nord()
        {
            _screenManager.LoadScreen(new chato_int_chambres_nord(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreenchato_ext_cours_interieur()
        {
            _screenManager.LoadScreen(new chato_ext_cours_interieur(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
    }
}