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
        public static Vector2 _cameraPosition;
        public int _numEcran;

        //Musiques
        private Song _songChato;
        private Song _titleTheme;
        private Song _songCombat;

        //Combat?
        private bool _combatTest;

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
            _graphics.PreferredBackBufferWidth = 768;
            _graphics.PreferredBackBufferHeight = 672;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphics.ApplyChanges();

            //Camera
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 512, 448);
            _camera = new OrthographicCamera(viewportadapter);
            _cameraPosition = new Vector2(chato_int_chambres_nord._positionPerso.X, chato_int_chambres_couloir._positionPerso.Y);
            _numEcran = 0;

            _combatTest = false;

            base.Initialize();

            //premier écran
            LoadScreenecran_de_titre();
        }

        protected override void LoadContent()
        {
            //Jsp
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Musiques
            _songChato = Content.Load<Song>("music/chato/EdgarAndSabin");
            _titleTheme = Content.Load<Song>("music/title/title");
            _songCombat = Content.Load<Song>("music/chato/GUERRE");
        }

        protected override void Update(GameTime gameTime)
        {
            //Mannette?
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _keyboardState = Keyboard.GetState();

            if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == false)
            {
                LoadScreenchato_combat();
                _combatTest = true;
            }
            else if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == true)
            {
                LoadScreenchato_int_chambres_nord();
                _combatTest = false;
            }

            //Camera
            if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambre1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre1;
            else if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambre2)
                _cameraPosition = chato_int_chambres_nord._chambreCentre2;
            else if (_numEcran == 2)
                _cameraPosition = new Vector2(chato_int_chambres_couloir._positionPerso.X, chato_int_chambres_couloir._positionPerso.Y);
            else if (_numEcran == 3)
                _cameraPosition = new Vector2(chato_ext_cours_interieur._positionPerso.X, chato_ext_cours_interieur._positionPerso.Y);
            else
                _cameraPosition = new Vector2(384, 336);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

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

        public void LoadScreenchato_int_chambres_nord()
        {
            _screenManager.LoadScreen(new chato_int_chambres_nord(this), new FadeTransition(GraphicsDevice, Color.Black));
            MediaPlayer.Play(_songChato);
            _numEcran = 1;
        }

        public void LoadScreenchato_int_chambres_couloir()
        {
            _screenManager.LoadScreen(new chato_int_chambres_couloir(this), new FadeTransition(GraphicsDevice, Color.Black));
            _numEcran = 2;
        }

        public void LoadScreenchato_ext_cours_interieur()
        {
            _screenManager.LoadScreen(new chato_ext_cours_interieur(this), new FadeTransition(GraphicsDevice, Color.Black));
            _numEcran = 3;
        }

        public void LoadScreenchato_combat()
        {
            MediaPlayer.Stop();
            _screenManager.LoadScreen(new chato_combat(this), new FadeTransition(GraphicsDevice, Color.Black));
            MediaPlayer.Play(_songCombat);
            _numEcran = 4;
        }
    }
}