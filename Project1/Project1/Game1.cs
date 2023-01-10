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
        //Graphique
        private GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;
        public static KeyboardState _keyboardState;

        // on définit écrans
        private Ecran_de_titre _ecranDeTitre;
        private Black_jack _blackJack;
        public Event_et_dial _eventEtDial;
        public Joueur _joueur;

        //Ecran interactif
        // on définit les différents états possibles du jeu
        public enum Etats { Menu, Start, Play, Quitter, Option };
        // on définit un champ pour stocker l'état en cours du jeu
        private Etats etat;



        //Ecran
        public static int xEcran;
        public static int yEcran;

        public static int xE;
        public static int yE;

        public static int changement0 = 1;
        public static double changement1 = 1.5;
        public static int changement2 = 2;
        public static double chan = 1;

        //Camera
        public OrthographicCamera _camera;
        public static OrthographicCamera _cameraDial;
        public static Vector2 _cameraPosition;
        public static int _numEcran;

        //Musiques
        private Song _songChato;
        private Song _titleTheme;
        public static Song _songCombat;
        public static Song _songDodo;

        //ZoneMusique
        private bool _chato;
        private bool _chatoCombat;
        private bool _ecranTitre;

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
        public static SoundEffect _wbeg;
        public static SoundEffect _wend;
        public static SoundEffect _toink;

        //Combat?
        private bool _combatTest;

        //Control cooldown 0.2s
        public static float _cooldown;
        public bool _cooldownVerif;
        public static float deltaSeconds;
        //Combat cooldown 0.5s
        public static float _cooldownC;
        public static bool _cooldownVerifC;

        //font
        public SpriteFont _font;

        //event
        public static bool _firstvisit;
        public static int _fin;

        //pour évènements et déplacementss
        public static float _walkSpeed;
        public static float _speed;
        public static TiledMap _tiledMap;
        public static Vector2 _positionPerso;
        public static TiledMapTileLayer mapLayer;
        public static int _stop;
        public static String _animationPlayer;
        public static TiledMapTileLayer mapLayerDoor;
        //public static TiledMapTileLayer mapIntersect;

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
            xEcran = 514; //768
            yEcran = 448; //672
            xE = xEcran;
            yE = yEcran;
            

            _graphics.PreferredBackBufferWidth = xEcran;
            _graphics.PreferredBackBufferHeight = yEcran;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphics.ApplyChanges();

            // on charge les 2 écrans 
            _ecranDeTitre = new Ecran_de_titre(this);
            _eventEtDial = new Event_et_dial(this);
            _blackJack = new Black_jack(this);
            _joueur = new Joueur(this);

            // Par défaut, le 1er état flèche l'écran de menu
            Etat = Etats.Menu;

            //Zone jeu
            _chato = false;
            _ecranTitre = false;
            _chatoCombat = false;

            //Camera

            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 514, 448);
            _camera = new OrthographicCamera(viewportadapter);

            var viewportadapterDial = new BoxingViewportAdapter(Window, GraphicsDevice, 514, 448);
            _cameraDial = new OrthographicCamera(viewportadapterDial);

            _cameraPosition = Chato_int_chambres_nord._chambreCentre1;
            _numEcran = 1;


            //Dialogue
            _eventEtDial._posText = new Vector2(105, 360);
            _eventEtDial._posNom = new Vector2(25, 360);
            _eventEtDial._posDialBox = new Vector2(0, 348);
            _eventEtDial._dialTrue = false;
            _eventEtDial._posChoiceBox = new Vector2(450, 284);
            _eventEtDial._posCursor = new Vector2(430, 316);
            _eventEtDial._posYes = new Vector2(470, 300);
            _eventEtDial._posNo = new Vector2(470, 315);
            _eventEtDial._yes = "oui";
            _eventEtDial._no = "non";


            //Combat?
            _combatTest = false;

            //event
            _firstvisit = true;
            _fin = 0;
            _speed = 100;
            _animationPlayer = "idle_down";
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Jsp
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Musiques
            _songChato = Content.Load<Song>("music/chato/chato");
            _titleTheme = Content.Load<Song>("music/title/title");
            _songCombat = Content.Load<Song>("music/chato/combat");
            _songDodo = Content.Load<Song>("music/fins/sleep");

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
            _wbeg = Content.Load<SoundEffect>("sfx/wbeg");
            _wend = Content.Load<SoundEffect>("sfx/wend");
            _toink = Content.Load<SoundEffect>("sfx/toink");

            //Boite de dialogue
            _eventEtDial._dialBox = Content.Load<Texture2D>("img/dialogue/dialogue_box");
            _eventEtDial._choiceBox = Content.Load<Texture2D>("img/dialogue/choice_box");
            _eventEtDial._cursor = Content.Load<Texture2D>("img/dialogue/cursor");

            //font
            _font = Content.Load<SpriteFont>("font/font_test");



            // on charge l'écran de menu par défaut 
            LoadScreenecran_de_titre();

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            //Mannette?
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _keyboardState = Keyboard.GetState();
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Test clic de souris + Etat 
            MouseState _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (this.Etat == Etats.Quitter)
                    Exit();

                else if (this.Etat == Etats.Start)
                    LoadScreenblack_jack();

                else if (this.Etat == Etats.Option)
                    LoadScreenoption();

                else
                    Console.WriteLine("");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                if (this.Etat == Etats.Start || this.Etat == Etats.Option || this.Etat == Etats.Menu)
                    LoadScreenecran_de_titre();

            //Console.WriteLine(_cooldownVerif);

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

            if (_cooldownVerifC == true)
            {
                _cooldownC = _cooldownC - deltaSeconds;
                if (_cooldownC <= 0)
                    _cooldownVerifC = false;
            }
            //Console.WriteLine(_cooldownVerifC);
            //Console.WriteLine(_cooldownC);


            // changement de dimension d'écran
            /*if (_keyboardState.IsKeyDown(Keys.E))
            {
                _graphics.PreferredBackBufferWidth = 514;
                _graphics.PreferredBackBufferHeight = 448;
                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                xE = (int)(xEcran * changement0);
                yE = (int)(yEcran * changement0);
                chan = changement0;
                _graphics.ApplyChanges();

            }
            if (_keyboardState.IsKeyDown(Keys.D))
            {
                _graphics.PreferredBackBufferWidth = (int)(xEcran * changement1);
                _graphics.PreferredBackBufferHeight = (int)(yEcran * changement1);
                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                xE = (int)(xEcran * changement1);
                yE = (int)(yEcran * changement1);
                chan = changement1;
                _graphics.ApplyChanges();
            }*/

            if (_keyboardState.IsKeyDown(Keys.F))
            {
                ChangementEcran(changement1);
            }


            //Camera

            // chambres nord
            if (_numEcran == 1 && Game1._positionPerso.X < Chato_int_chambres_nord._limiteChambreX1
                                && Game1._positionPerso.Y < Chato_int_chambres_nord._limiteChambreY1
                                && Game1._positionPerso.X < Chato_int_chambres_nord._limiteChambreGauche)
                _cameraPosition = Chato_int_chambres_nord._chambreCentre1;

            else if (_numEcran == 1 && Game1._positionPerso.X < Chato_int_chambres_nord._limiteChambreX1
                                && Game1._positionPerso.Y < Chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = Chato_int_chambres_nord._chambreCentreUn;

            else if (_numEcran == 1 && Game1._positionPerso.X > Chato_int_chambres_nord._limiteChambreX2
                                && Game1._positionPerso.Y < Chato_int_chambres_nord._limiteChambreY1
                                && Game1._positionPerso.X > Chato_int_chambres_nord._limiteChambreDroite)
                _cameraPosition = Chato_int_chambres_nord._chambreCentreDeux;

            else if (_numEcran == 1 && Game1._positionPerso.X > Chato_int_chambres_nord._limiteChambreX2 
                                && Game1._positionPerso.Y < Chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = Chato_int_chambres_nord._chambreCentre2;

            else if (_numEcran == 1 && Game1._positionPerso.Y >= Chato_int_chambres_couloir._limiteCouloirY1)
                _cameraPosition = new Vector2(Game1._positionPerso.X, Game1._positionPerso.Y);


            // couloir
            if (_numEcran == 2 && (Game1._positionPerso.Y > 0
                                && (Game1._positionPerso.X > Chato_int_chambres_couloir._limiteChambreX1 ||
                                Game1._positionPerso.X < Chato_int_chambres_couloir._limiteChambreX2)))
                _cameraPosition = new Vector2(Game1._positionPerso.X, Game1._positionPerso.Y);

            else if (_numEcran == 2 && Game1._positionPerso.X < Chato_int_chambres_nord._limiteChambreX1
                                && Game1._positionPerso.X < Chato_int_chambres_nord._limiteChambreGauche
                                && Game1._positionPerso.Y >= Chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = Chato_int_chambres_nord._chambreCentre1;

            else if (_numEcran == 2 && Game1._positionPerso.X < Chato_int_chambres_nord._limiteChambreX1 
                                && Game1._positionPerso.X > Chato_int_chambres_nord._limiteChambreGauche
                                && Game1._positionPerso.Y >= Chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = Chato_int_chambres_nord._chambreCentreUn;

            else if (_numEcran == 2 && Game1._positionPerso.X > Chato_int_chambres_nord._limiteChambreX2
                                && Game1._positionPerso.X < Chato_int_chambres_nord._limiteChambreDroite
                                && Game1._positionPerso.Y >= Chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = Chato_int_chambres_nord._chambreCentre2;

            else if (_numEcran == 2 && Game1._positionPerso.X > Chato_int_chambres_nord._limiteChambreX2
                                && Game1._positionPerso.X > Chato_int_chambres_nord._limiteChambreDroite
                                && Game1._positionPerso.Y >= Chato_int_chambres_nord._limiteChambreY1)
                _cameraPosition = Chato_int_chambres_nord._chambreCentreDeux;

            else if (_numEcran == 2 && Game1._positionPerso.Y > 49*16)
                _cameraPosition = new Vector2(Game1._positionPerso.X, Game1._positionPerso.Y);

            // cours
            else if (_numEcran == 3 & Game1._positionPerso.Y >= 1 * 16)
                _cameraPosition = new Vector2(Game1._positionPerso.X, Game1._positionPerso.Y);

            else if (_numEcran == 3 && Game1._positionPerso.Y < 1*16 )
                _cameraPosition = new Vector2(Game1._positionPerso.X, Game1._positionPerso.Y);

            // combat
            else if (_numEcran == 4)
                _cameraPosition = Chato_combat._centreCombat;

            _walkSpeed = _speed * deltaSeconds;
            Console.WriteLine(Chato_int_chambres_nord._posX);
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
            _screenManager.LoadScreen(new Ecran_de_titre(this), new FadeTransition(GraphicsDevice, Color.LightGray));
            MusiqueTitre();
            this.Etat = Etats.Menu;
        }

        public void LoadScreenoption()
        {
            _screenManager.LoadScreen(new Option(this), new FadeTransition(GraphicsDevice, Color.LightGray));
            MusiqueTitre();
            this.Etat = Etats.Option;
        }

        public void LoadScreenblack_jack()
        {
            MediaPlayer.Stop();
            _ecranTitre = false;
            _screenManager.LoadScreen(new Black_jack(this), new FadeTransition(GraphicsDevice, Color.Black));
            this.Etat = Etats.Play;
        }

        public void LoadScreenchato_int_chambres_nord()
        {
            _screenManager.LoadScreen(new Chato_int_chambres_nord(this), new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            _ecranTitre=false;
            _numEcran = 1;
            _chatoCombat = false;
        }

        public void LoadScreenchato_int_chambres_couloir()
        {
            _screenManager.LoadScreen(new Chato_int_chambres_couloir(this), new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            _numEcran = 2;
            _chatoCombat = false;
        }

        public void LoadScreenchato_ext_cours_interieur()
        {
            _screenManager.LoadScreen(new Chato_ext_cours_interieur(this), new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChato();
            _numEcran = 3;
            _chatoCombat = false;
        }

        public void LoadScreenchato_combat()
        {
            MediaPlayer.Stop();
            _screenManager.LoadScreen(new Chato_combat(this), new FadeTransition(GraphicsDevice, Color.Black));
            MusiqueChatoCombat();
            MediaPlayer.Play(_songCombat);
            _numEcran = 4;
            _chato = false;
            _chatoCombat = true;
        }

        //autre
        public void SetCoolDown()
        {
            _cooldownVerif = true;
            _cooldown = 0.2f;
            _menu.Play();
        }

        public static void SetCoolDownCombat()
        {
            _cooldownVerifC = true;
            _cooldownC = 0.5f;
        }

        public void MusiqueChatoCombat()
        {
            if (_chatoCombat == false)
            {
                MediaPlayer.Stop();
                _chatoCombat = true;
                MediaPlayer.Play(_songCombat);
            }
        }

        public void MusiqueChato()
        {
            if (_chato == false)
            {
                MediaPlayer.Stop();
                _chato = true;
                MediaPlayer.Play(_songChato);
            }
        }

        public void MusiqueTitre()
        {
            if (_ecranTitre == false)
            {
                MediaPlayer.Stop();
                _ecranTitre = true;
                MediaPlayer.Play(_titleTheme);
            }
        }

        public void ChangementEcran(double changement)
        {
            _graphics.PreferredBackBufferWidth = (int)(xEcran * changement);
            _graphics.PreferredBackBufferHeight = (int)(yEcran * changement);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            xE = (int)(xEcran * changement);
            yE = (int)(yEcran * changement);
            chan = changement;
            _graphics.ApplyChanges();
        }
    }
}