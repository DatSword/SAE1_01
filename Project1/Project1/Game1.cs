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
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace SAE101
{
    public class Game1 : Game
    {
        //honnêtement j'en sais r
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;
        private KeyboardState _keyboardState;

        //Camera
        public static OrthographicCamera _camera;
        //private BoxingViewportAdapter _viewport;

        //Musiques
        private Song _songChato;
        private Song _titleTheme;

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
            _graphics.PreferredBackBufferWidth = 512;
            _graphics.PreferredBackBufferHeight = 448;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphics.ApplyChanges();

            base.Initialize();

            //premier écran
            LoadScreenecran_de_titre();

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 640, 160);
            //_viewport = new BoxingViewportAdapter(this.Window, this.GraphicsDevice, 1024, 896); //Zoom*3
            _camera = new OrthographicCamera(viewportAdapter);
        }

        protected override void LoadContent()
        {
            //Jsp
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Musiques
            _songChato = Content.Load<Song>("music/chato/EdgarAndSabin");
            _titleTheme = Content.Load<Song>("music/title/title");
        }

        /*private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }
            return movementDirection;
        }*/

        protected override void Update(GameTime gameTime)
        {
            //Mannette?
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _keyboardState = Keyboard.GetState();

            /*const float movementSpeed = 200;
            _camera.Move(GetMovementDirection() * movementSpeed); //* gameTime.GetElapsedSeconds());*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            /*var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            _spriteBatch.DrawRectangle(new RectangleF(100, 100, 50, 50), Color.Red, 3f);
            _spriteBatch.End();*/

            base.Draw(gameTime);
        }

        public void LoadScreenecran_de_titre()
        {
            _screenManager.LoadScreen(new ecran_de_titre(this), new FadeTransition(GraphicsDevice, Color.BlueViolet));
            MediaPlayer.Play(_titleTheme);
        }

        public void LoadScreenblack_jack()
        {
            MediaPlayer.Stop();
            _screenManager.LoadScreen(new black_jack(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreenchato_int_chambres_couloir()
        {
            _screenManager.LoadScreen(new chato_int_chambres_couloir(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreenchato_int_chambres_nord()
        {
            _screenManager.LoadScreen(new chato_int_chambres_nord(this), new FadeTransition(GraphicsDevice, Color.Black));
            MediaPlayer.Play(_songChato);
        }

        public void LoadScreenchato_ext_cours_interieur()
        {
            _screenManager.LoadScreen(new chato_ext_cours_interieur(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreenchato_combat()
        {
            _screenManager.LoadScreen(new chato_combat(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
    }
}