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
using Microsoft.Xna.Framework.Audio;

namespace SAE101
{
    public class Game1 : Game
    {
        //honnêtement j'en sais r
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;
        public static KeyboardState _keyboardState;

        //Camera
        public static OrthographicCamera _camera;
        public static Vector2 _cameraPosition;
        public static int _numEcran;

        //Musiques
        private Song _songChato;
        private Song _titleTheme;
        private Song _songCombat;

        //SFX
        public static SoundEffect _hit;
        public static SoundEffect _hit2;
        public static SoundEffect _lose;
        public static SoundEffect _macron_1;
        public static SoundEffect _menu;
        public static SoundEffect _non;
        public static SoundEffect _pelo;
        public static SoundEffect _vic;

        //Combat?
        private bool _combatTest;

        //Control
        public static float _cooldown;
        public static bool _cooldownVerif;
        public static float deltaSeconds;

        //Boites de dialogues
        public static Texture2D _dialBox;
        public static Vector2 _posDialBox;
        public static String _text;
        public static Vector2 _posText;
        public static String _nom;
        public static Vector2 _posNom;
        public static bool _dialTrue;

        //font
        public static SpriteFont _font;

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
            _numEcran = 1;

            //Dialogue
            _posText = new Vector2(105, 360);
            _posNom = new Vector2(25, 360);
            _posDialBox = new Vector2(0, 348);          
            _dialTrue = false;

            //Combat?
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

            //SFX
            _menu = Content.Load<SoundEffect>("sfx/menu");
            _non = Content.Load<SoundEffect>("sfx/non");
            _hit = Content.Load<SoundEffect>("sfx/hit");
            _hit2 = Content.Load<SoundEffect>("sfx/hit2");
            _macron_1 = Content.Load<SoundEffect>("sfx/macron_1");
            _pelo = Content.Load<SoundEffect>("sfx/pelo");
            _vic = Content.Load<SoundEffect>("sfx/vic");
            _non = Content.Load<SoundEffect>("sfx/non");

            //Boite de dialogue
            _dialBox = Content.Load<Texture2D>("img/dialogue/dialogue_box");

            //font
            _font = Content.Load<SpriteFont>("font/font_test");

        }





        protected override void Update(GameTime gameTime)
        {
            //Mannette?
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _keyboardState = Keyboard.GetState();
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Console.WriteLine(_cooldownVerif);
            if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == false && _cooldownVerif == false)
            {
                LoadScreenchato_combat();
                _combatTest = true;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }
            else if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == true && _cooldownVerif == false)
            {
                LoadScreenchato_int_chambres_nord();
                _combatTest = false;
                _cooldownVerif = true;
                _cooldown = 0.2f;
            }

            if (_cooldownVerif == true)
            {
                _cooldown = _cooldown - deltaSeconds;
                if (_cooldown <= 0)
                    _cooldownVerif = false;
            }


            //Camera
            if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreX1
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre1;

            if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreX1
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1
                                && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreGauche)
                _cameraPosition = chato_int_chambres_nord._chambreCentreUn;


            else if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreX2 
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre2;

            else if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreX2
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1
                                && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreDroite)
                _cameraPosition = chato_int_chambres_nord._chambreCentreDeux;


            else if (_numEcran == 1 && chato_int_chambres_couloir._positionPerso.Y >= chato_int_chambres_couloir._limiteCouloirY1)
                _cameraPosition = new Vector2(chato_int_chambres_couloir._positionPerso.X, chato_int_chambres_couloir._positionPerso.Y);



            else if (_numEcran == 2 
                                && (chato_int_chambres_couloir._positionPerso.Y > 0
                                && (chato_int_chambres_couloir._positionPerso.X > chato_int_chambres_couloir._limiteChambreX1 || 
                                chato_int_chambres_couloir._positionPerso.X < chato_int_chambres_couloir._limiteChambreX2)))
                _cameraPosition = new Vector2(chato_int_chambres_couloir._positionPerso.X, chato_int_chambres_couloir._positionPerso.Y);

            else if (_numEcran == 2
                                && (chato_int_chambres_couloir._positionPerso.Y > chato_int_chambres_couloir._limiteCouloirY1
                                && (chato_int_chambres_couloir._positionPerso.X > chato_int_chambres_couloir._limiteChambreX1 ||
                                chato_int_chambres_couloir._positionPerso.X < chato_int_chambres_couloir._limiteChambreX2)))
                _cameraPosition = new Vector2(chato_int_chambres_couloir._positionPerso.X, chato_int_chambres_couloir._positionPerso.Y);


            else if (_numEcran == 2 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreX1
                                && chato_int_chambres_nord._positionPerso.Y >= chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre1;

            else if (_numEcran == 2 && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreX2
                                && chato_int_chambres_nord._positionPerso.Y >= chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre2;



            else if (_numEcran == 3)
                _cameraPosition = new Vector2(chato_ext_cours_interieur._positionPerso.X, chato_ext_cours_interieur._positionPerso.Y);
            
            else if (_numEcran == 4)
                _cameraPosition = chato_combat._centreCombat;

            /*else
                _cameraPosition*/



            Console.WriteLine(_numEcran);

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