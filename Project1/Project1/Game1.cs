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
        public static SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;
        public static KeyboardState _keyboardState;


        // on définit les différents états possibles du jeu ( à compléter) 
        public enum Etats { Menu, Play, Quitter, Option };

        // on définit un champ pour stocker l'état en cours du jeu
        private Etats etat;

        // on définit  2 écrans ( à compléter )
        private ecran_de_titre _ecranDeTitre;
        private black_jack _blackJack;


        //Camera
        public static OrthographicCamera _camera;
        public static OrthographicCamera _cameraDial;
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
        public static SoundEffect _duck;

        //Combat?
        private bool _combatTest;

        //Control
        public static float _cooldown;
        public static bool _cooldownVerif;
        public static float deltaSeconds;

        //font
        public static SpriteFont _font;

        //event
        public static bool _firstvisit;

        public Etats Etat
        {
            get
            {   return this.etat;   }
            set
            {   this.etat = value;  }
        }

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


            // Par défaut, le 1er état flèche l'écran de menu
            Etat = Etats.Menu;
            // on charge les 2 écrans 
            _ecranDeTitre = new ecran_de_titre(this);
            _blackJack = new black_jack(this);


            //Camera

            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 514, 448);
            _camera = new OrthographicCamera(viewportadapter);

            var viewportadapterDial = new BoxingViewportAdapter(Window, GraphicsDevice, 514, 448);
            _cameraDial = new OrthographicCamera(viewportadapterDial);

            _cameraPosition = chato_int_chambres_nord._chambreCentre1;
            _numEcran = 1;


            //Dialogue
            eventsetdial._posText = new Vector2(105, 360);
            eventsetdial._posNom = new Vector2(25, 360);
            eventsetdial._posDialBox = new Vector2(0, 348);
            eventsetdial._dialTrue = false;


            //Combat?
            _combatTest = false;

            //event
            _firstvisit = true;

            base.Initialize();

            //premier écran
            LoadScreenecran_de_titre();
        }

        protected override void LoadContent()
        {
            //Jsp
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // on charge l'écran de menu par défaut 
            _screenManager.LoadScreen(_ecranDeTitre, new FadeTransition(GraphicsDevice, Color.Black));

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
            _duck = Content.Load<SoundEffect>("sfx/duck");

            //Boite de dialogue
            eventsetdial._dialBox = Content.Load<Texture2D>("img/dialogue/dialogue_box");

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

            // On teste le clic de souris et l'état pour savoir quelle action faire 
            MouseState _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                // Attention, l'état a été mis à jour directement par l'écran en question
                if (this.Etat == Etats.Quitter)
                    Exit();

                else if (this.Etat == Etats.Play)
                    LoadScreenblack_jack();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                if (this.Etat == Etats.Menu)
                    LoadScreenecran_de_titre();
            }

            Console.WriteLine(_cooldownVerif);

            if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == false && _cooldownVerif == false)
            {
                LoadScreenchato_combat();
                _combatTest = true;
                SetCoolDown();
            }
            else if (_keyboardState.IsKeyDown(Keys.C) && _combatTest == true && _cooldownVerif == false)
            {
                LoadScreenchato_int_chambres_nord();
                _combatTest = false;
                SetCoolDown();
            }

            if (_cooldownVerif == true)
            {
                _cooldown = _cooldown - deltaSeconds;
                if (_cooldown <= 0)
                    _cooldownVerif = false;
            }


            //Camera

                /* chambres nord */
            Console.WriteLine(_numEcran);

            if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreX1
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1
                                && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreGauche)
                _cameraPosition = chato_int_chambres_nord._chambreCentre1;

            else if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreX1
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentreUn;

            else if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreX2
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1
                                && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreDroite)
                _cameraPosition = chato_int_chambres_nord._chambreCentreDeux;

            else if (_numEcran == 1 && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreX2 
                                && chato_int_chambres_nord._positionPerso.Y < chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre2;

            else if (_numEcran == 1 && chato_int_chambres_couloir._positionPerso.Y >= chato_int_chambres_couloir._limiteCouloirY1)
                _cameraPosition = new Vector2(chato_int_chambres_couloir._positionPerso.X, chato_int_chambres_couloir._positionPerso.Y);


                /* couloir */
            if (_numEcran == 2 && (chato_int_chambres_couloir._positionPerso.Y > 0
                                && (chato_int_chambres_couloir._positionPerso.X > chato_int_chambres_couloir._limiteChambreX1 || 
                                chato_int_chambres_couloir._positionPerso.X < chato_int_chambres_couloir._limiteChambreX2)))
                _cameraPosition = new Vector2(chato_int_chambres_couloir._positionPerso.X, chato_int_chambres_couloir._positionPerso.Y);

            else if (_numEcran == 2 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreX1
                                && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreGauche
                                && chato_int_chambres_nord._positionPerso.Y >= chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre1;

            else if (_numEcran == 2 && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreX1 
                                && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreGauche
                                && chato_int_chambres_nord._positionPerso.Y >= chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentreUn;

            else if (_numEcran == 2 && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreX2
                                && chato_int_chambres_nord._positionPerso.X < chato_int_chambres_nord._limiteChambreDroite
                                && chato_int_chambres_nord._positionPerso.Y >= chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentre2;

            else if (_numEcran == 2 && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreX2
                                && chato_int_chambres_nord._positionPerso.X > chato_int_chambres_nord._limiteChambreDroite
                                && chato_int_chambres_nord._positionPerso.Y >= chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = chato_int_chambres_nord._chambreCentreDeux;

            else if (_numEcran == 2 && chato_ext_cours_interieur._positionPerso.Y > 49*16)
                _cameraPosition = new Vector2(chato_ext_cours_interieur._positionPerso.X, chato_ext_cours_interieur._positionPerso.Y);

            else if (_numEcran == 3 & chato_int_chambres_nord._positionPerso.Y >= 1 * 16)
                _cameraPosition = new Vector2(chato_ext_cours_interieur._positionPerso.X, chato_ext_cours_interieur._positionPerso.Y);

            else if (_numEcran == 3 && chato_int_chambres_nord._positionPerso.Y < 1*16 )
                _cameraPosition = new Vector2(chato_ext_cours_interieur._positionPerso.X, chato_ext_cours_interieur._positionPerso.Y);

            
            else if (_numEcran == 4)
                _cameraPosition = chato_combat._centreCombat;


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }


        //Chargements maps
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

        //autre

        public static void SetCoolDown()
        {
            _cooldownVerif = true;
            _cooldown = 0.15f;
            _menu.Play();
        }
    }
}